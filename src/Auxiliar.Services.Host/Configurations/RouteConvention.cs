using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Auxiliar.Configurations
{
    public class RouteConvention : IApplicationModelConvention
    {
        #region Campos Privados

        private readonly AttributeRouteModel _centralPrefix;

        #endregion Campos Privados

        #region Construtores Publicos

        public RouteConvention(IRouteTemplateProvider routeTemplateProvider)
        {
            _centralPrefix = new AttributeRouteModel(routeTemplateProvider);
        }

        #endregion Construtores Publicos

        #region Metodos Publicos

        public void Apply(ApplicationModel application)
        {
            foreach (ControllerModel controller in application.Controllers)
            {
                List<SelectorModel> _matchedSelectors = controller.Selectors.Where(selectorModel => selectorModel.AttributeRouteModel != null)
                                                                            .ToList();

                if (_matchedSelectors.Any())
                    foreach (SelectorModel selectorModel in _matchedSelectors)
                        selectorModel.AttributeRouteModel = AttributeRouteModel.CombineAttributeRouteModel(left: _centralPrefix,
                                                                                                           right: selectorModel.AttributeRouteModel);

                List<SelectorModel> _unmatchedSelectors = controller.Selectors.Where(selectorModel => selectorModel.AttributeRouteModel is null)
                                                                              .ToList();

                if (_unmatchedSelectors.Any())
                    foreach (SelectorModel selectorModel in _unmatchedSelectors)
                        selectorModel.AttributeRouteModel = _centralPrefix;
            }
        }

        #endregion Metodos Publicos
    }

    public static class MvcOptionsExtensions
    {
        #region Metodos Publicos

        public static void UseCentralRoutePrefix(this MvcOptions opts, IRouteTemplateProvider routeAttribute)
        {
            opts.Conventions.Insert(index: 0,
                                    item: new RouteConvention(routeAttribute));
        }

        #endregion Metodos Publicos
    }
}