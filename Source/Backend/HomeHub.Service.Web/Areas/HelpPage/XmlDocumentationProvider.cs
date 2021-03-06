namespace HomeHub.Service.Web.Areas.HelpPage
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Web.Http.Controllers;
    using System.Web.Http.Description;
    using System.Xml.XPath;

    using HomeHub.Service.Web.Areas.HelpPage.ModelDescriptions;

    /// <summary>
    ///     A custom <see cref="IDocumentationProvider" /> that reads the API documentation from an XML documentation file.
    /// </summary>
    public class XmlDocumentationProvider : IDocumentationProvider, IModelDocumentationProvider
    {
        private const string FieldExpression = "/doc/members/member[@name='F:{0}']";

        private const string MethodExpression = "/doc/members/member[@name='M:{0}']";

        private const string ParameterExpression = "param[@name='{0}']";

        private const string PropertyExpression = "/doc/members/member[@name='P:{0}']";

        private const string TypeExpression = "/doc/members/member[@name='T:{0}']";

        private readonly IEnumerable<XPathNavigator> _documentNavigators;

        /// <summary>
        ///     Initializes a new instance of the <see cref="XmlDocumentationProvider" /> class.
        /// </summary>
        /// <param name="documentPaths">The physical path to XML documents.</param>
        public XmlDocumentationProvider(params string[] documentPaths)
        {
            if (false == documentPaths.Any())
            {
                throw new ArgumentNullException("documentPaths");
            }

            this._documentNavigators = documentPaths.Select(p => new XPathDocument(p).CreateNavigator());
        }

        public string GetDocumentation(HttpControllerDescriptor controllerDescriptor)
        {
            var typeNode = this.GetTypeNode(controllerDescriptor.ControllerType);
            return GetTagValue(typeNode, "summary");
        }

        /// <summary> Gets the documentation based on <see cref="T:System.Web.Http.Controllers.HttpActionDescriptor" />. </summary>
        /// <returns>The documentation for the controller.</returns>
        /// <param name="actionDescriptor">The action descriptor.</param>
        public virtual string GetDocumentation(HttpActionDescriptor actionDescriptor)
        {
            var methodNode = this.GetMethodNode(actionDescriptor);
            return GetTagValue(methodNode, "summary");
        }

        /// <summary> Gets the documentation based on <see cref="T:System.Web.Http.Controllers.HttpParameterDescriptor" />. </summary>
        /// <returns>The documentation for the controller.</returns>
        /// <param name="parameterDescriptor">The parameter descriptor.</param>
        public virtual string GetDocumentation(HttpParameterDescriptor parameterDescriptor)
        {
            var reflectedParameterDescriptor = parameterDescriptor as ReflectedHttpParameterDescriptor;
            if (reflectedParameterDescriptor != null)
            {
                var methodNode = this.GetMethodNode(reflectedParameterDescriptor.ActionDescriptor);
                if (methodNode != null)
                {
                    var parameterName = reflectedParameterDescriptor.ParameterInfo.Name;
                    var parameterNode =
                        methodNode.SelectSingleNode(
                            string.Format(CultureInfo.InvariantCulture, ParameterExpression, parameterName));
                    if (parameterNode != null)
                    {
                        return parameterNode.Value.Trim();
                    }
                }
            }

            return null;
        }

        public string GetDocumentation(MemberInfo member)
        {
            var memberName = string.Format(
                CultureInfo.InvariantCulture, 
                "{0}.{1}", 
                GetTypeName(member.DeclaringType), 
                member.Name);
            var expression = member.MemberType == MemberTypes.Field ? FieldExpression : PropertyExpression;
            var selectExpression = string.Format(CultureInfo.InvariantCulture, expression, memberName);
            var propertyNode = this.SelectSingleNode(selectExpression);
            return GetTagValue(propertyNode, "summary");
        }

        public string GetDocumentation(Type type)
        {
            var typeNode = this.GetTypeNode(type);
            return GetTagValue(typeNode, "summary");
        }

        public string GetResponseDocumentation(HttpActionDescriptor actionDescriptor)
        {
            var methodNode = this.GetMethodNode(actionDescriptor);
            return GetTagValue(methodNode, "returns");
        }

        private static string GetMemberName(MethodInfo method)
        {
            var name = string.Format(
                CultureInfo.InvariantCulture, 
                "{0}.{1}", 
                GetTypeName(method.DeclaringType), 
                method.Name);
            var parameters = method.GetParameters();
            if (parameters.Length != 0)
            {
                var parameterTypeNames = parameters.Select(param => GetTypeName(param.ParameterType)).ToArray();
                name += string.Format(CultureInfo.InvariantCulture, "({0})", string.Join(",", parameterTypeNames));
            }

            return name;
        }

        private static string GetTagValue(XPathNavigator parentNode, string tagName)
        {
            if (parentNode != null)
            {
                var node = parentNode.SelectSingleNode(tagName);
                if (node != null)
                {
                    return node.Value.Trim();
                }
            }

            return null;
        }

        /// <summary>
        /// Get the type name
        /// </summary>
        /// <param name="type">The type</param>
        /// <returns>Returns the type name</returns>
        private static string GetTypeName(Type type)
        {
            var name = type.FullName;
            if (type.IsGenericType)
            {
                // Format the generic type name to something like: Generic{System.Int32,System.String}
                var genericType = type.GetGenericTypeDefinition();
                var genericArguments = type.GetGenericArguments();
                var genericTypeName = genericType.FullName;

                // Trim the generic parameter counts from the name
                genericTypeName = genericTypeName.Substring(0, genericTypeName.IndexOf('`'));
                var argumentTypeNames = genericArguments.Select(t => GetTypeName(t)).ToArray();
                name = string.Format(
                    CultureInfo.InvariantCulture, 
                    "{0}{{{1}}}", 
                    genericTypeName, 
                    string.Join(",", argumentTypeNames));
            }

            if (type.IsNested)
            {
                // Changing the nested type name from OuterType+InnerType to OuterType.InnerType to match the XML documentation syntax.
                name = name.Replace("+", ".");
            }

            return name;
        }

        /// <summary>
        /// Get the node for a method
        /// </summary>
        /// <param name="actionDescriptor">The action descriptors</param>
        /// <returns>The x path navigator</returns>
        private XPathNavigator GetMethodNode(HttpActionDescriptor actionDescriptor)
        {
            var reflectedActionDescriptor = actionDescriptor as ReflectedHttpActionDescriptor;
            if (reflectedActionDescriptor != null)
            {
                var selectExpression = string.Format(
                    CultureInfo.InvariantCulture, 
                    MethodExpression, 
                    GetMemberName(reflectedActionDescriptor.MethodInfo));
                return this.SelectSingleNode(selectExpression);
            }

            return null;
        }

        /// <summary>
        /// Get the node for a type
        /// </summary>
        /// <param name="type">The type</param>
        /// <returns>The x path navigator</returns>
        private XPathNavigator GetTypeNode(Type type)
        {
            var controllerTypeName = GetTypeName(type);
            var selectExpression = string.Format(CultureInfo.InvariantCulture, TypeExpression, controllerTypeName);
            return this.SelectSingleNode(selectExpression);
        }

        /// <summary>
        /// Select a single node, search all of the documentation files
        /// </summary>
        /// <param name="expression">The query</param>
        /// <returns>The x path navigator</returns>
        private XPathNavigator SelectSingleNode(string expression)
        {
            foreach (var navigator in this._documentNavigators)
            {
                var result = navigator.SelectSingleNode(expression);
                if (null != result)
                {
                    return result;
                }
            }

            return null;
        }
    }
}