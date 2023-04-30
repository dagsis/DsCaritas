using Caritas.Web.Helpers;

namespace Caritas.Web.Extensions
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Assembly, AllowMultiple = true)]

    public class BreadcrumbAttribute : Attribute
    {
        public BreadcrumbAttribute(string label, string controller = "", string action = "", bool passArgs = false)
        {
            Label = label;
            ControllerName = controller;
            ActionName = action;
            PassArguments = passArgs;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BreadcrumbAttribute"/> class.
        /// </summary>
        /// <param name="resourceType">Type of the resource.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="controller">The controller.</param>
        /// <param name="action">The action.</param>
        /// <param name="passArgs">if set to <c>true</c> [pass arguments].</param>
        public BreadcrumbAttribute(Type resourceType, string resourceName, string controller = "", string action = "", bool passArgs = false)
        {
            Label = ResourceHelper.GetResourceLookup(resourceType, resourceName);
            ControllerName = controller;
            ActionName = action;
            PassArguments = passArgs;
        }

        /// <summary>
        /// Gets the label.
        /// </summary>
        /// <value>The label.</value>
        public string Label { get; }

        /// <summary>
        /// Gets the name of the controller.
        /// </summary>
        /// <value>The name of the controller.</value>
        public string ControllerName { get; }

        /// <summary>
        /// Gets the name of the action.
        /// </summary>
        /// <value>The name of the action.</value>
        public string ActionName { get; }

        /// <summary>
        /// Gets a value indicating whether [pass arguments].
        /// </summary>
        /// <value><c>true</c> if [pass arguments]; otherwise, <c>false</c>.</value>
        public bool PassArguments { get; }
    }
}
