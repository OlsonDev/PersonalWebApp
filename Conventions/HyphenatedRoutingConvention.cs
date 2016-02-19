using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.AspNet.Mvc.ApplicationModels;

namespace PersonalWebApp.Conventions {
	// See https://github.com/aspnet/Routing/issues/186
	public class HyphenatedRoutingConvention : IApplicationModelConvention {
		public string DefaultController { get; set; }
		public string DefaultAction { get; set; }

		public HyphenatedRoutingConvention(string defaultController = "home", string defaultAction = "index") {
			DefaultController = defaultController.ToLower();
			DefaultAction = defaultAction.ToLower();
		}

		public void Apply(ApplicationModel application) {
			foreach (var controller in application.Controllers) {
				if (controller.AttributeRoutes.Count != 0) continue;

				var controllerTmpl = Slugify(controller.ControllerName);
				controller.AttributeRoutes.Add(new AttributeRouteModel { Template = controllerTmpl });
				if (controllerTmpl == DefaultController) {
					controller.AttributeRoutes.Add(new AttributeRouteModel { Template = "" });
				}

				var actionsToAdd = new List<ActionModel>();
				foreach (var action in controller.Actions) {
					if (action.AttributeRouteModel != null) continue;
					var actionSlug = Slugify(action.ActionName);
					var actionTmpl = $"{actionSlug}/{{id?}}";
					if (actionSlug == DefaultAction) {
						var defaultActionModel = new ActionModel(action) { AttributeRouteModel = new AttributeRouteModel { Template = "" } };
						actionsToAdd.Add(defaultActionModel);
					}
					action.AttributeRouteModel = new AttributeRouteModel { Template = actionTmpl };
				}
				// Flaky; contigent on resolution of https://github.com/aspnet/Mvc/issues/4043
				foreach (var action in actionsToAdd) {
					controller.Actions.Add(action);
				}
			}
		}

		private string Slugify(string pascalString) {
			return Regex.Replace(pascalString, "([a-z])([A-Z])", "$1-$2").ToLower();
		}
	}
}