using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using PersonalWebApp.Extensions;
using System.Linq;

namespace PersonalWebApp.Conventions {
	// See https://github.com/aspnet/Routing/issues/186
	public class HyphenatedRoutingConvention : IApplicationModelConvention {
		public string DefaultController { get; set; }
		public string DefaultAction { get; set; }

		public HyphenatedRoutingConvention(string defaultController = "blog", string defaultAction = "index") {
			DefaultController = defaultController.ToLower();
			DefaultAction = defaultAction.ToLower();
		}

		public void Apply(ApplicationModel application) {
			foreach (var controller in application.Controllers) {
				var hasAttributeRouteModels = controller.Selectors.Any(selector => selector.AttributeRouteModel != null);
				if (hasAttributeRouteModels) continue;

				var controllerTmpl = controller.ControllerName.PascalToSlug();
				controller.Selectors.Add(new SelectorModel { AttributeRouteModel = new AttributeRouteModel { Template = controllerTmpl } });

				var actionsToAdd = new List<ActionModel>();
				foreach (var action in controller.Actions) {
					var hasAttributeRouteModel = action.Selectors.Any(selector => selector.AttributeRouteModel != null);
					if (hasAttributeRouteModel) continue;
					var actionSlug = action.ActionName.PascalToSlug();
					var actionTmpl = $"{actionSlug}/{{id?}}";
					if (actionSlug == DefaultAction) {
						var defaultActionModel = new ActionModel(action);
						defaultActionModel.Selectors.Add(new SelectorModel { AttributeRouteModel = new AttributeRouteModel { Template = "" } });
						actionsToAdd.Add(defaultActionModel);
						if (controllerTmpl == DefaultController) {
							var defaultControllerModel = new ActionModel(action);
							defaultControllerModel.Selectors.Add(new SelectorModel { AttributeRouteModel = new AttributeRouteModel { Template = "/" } });
							actionsToAdd.Add(defaultControllerModel);
						}
					}
					action.Selectors.Add(new SelectorModel { AttributeRouteModel = new AttributeRouteModel { Template = actionTmpl } });
				}
				// Flaky; contigent on resolution of https://github.com/aspnet/Mvc/issues/4043
				foreach (var action in actionsToAdd) {
					controller.Actions.Add(action);
				}
			}
		}
	}
}