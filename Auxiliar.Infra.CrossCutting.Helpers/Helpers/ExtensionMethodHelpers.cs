using Auxiliar.Domain.Core.Attributes;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net;

namespace Auxiliar.Infra.CrossCutting.Helpers
{
    public static class ExtensionMethodHelpers
    {
        #region Metodos Publicos

        /// <summary>
        /// Obter o valor da Annotation StatusCode: [StatusCode(500)]
        /// </summary>
        /// <typeparam name="TType">automaticamente obtido com o objeto.</typeparam>
        /// <param name="myEnum">objeto do tipo Enum.</param>
        /// <returns></returns>
        public static HttpStatusCode GetStatusCode<TType>(this TType myEnum)
        {
            var _fieldInfo = myEnum.GetType().GetField(myEnum.ToString());

            var _customAttributes = (HttpStatusCodeAttribute[])_fieldInfo.GetCustomAttributes(
                typeof(HttpStatusCodeAttribute), false);

            return
                   _customAttributes != null && _customAttributes.Any()
                       ? _customAttributes[0].HttpStatusCode
                       : HttpStatusCode.InternalServerError;
        }

        /// <summary>
        /// Obter o valor da Annotation Description: [Description("Minha Descrição")]
        /// </summary>
        /// <typeparam name="TType">automaticamente obtido com o objeto.</typeparam>
        /// <param name="myEnum">objeto do tipo Enum.</param>
        /// <returns></returns>
        public static string GetDescription<TType>(this TType myEnum)
        {
            System.Reflection.FieldInfo _fieldInfo = myEnum.GetType().GetField(myEnum.ToString());

            DescriptionAttribute[] _customAttributes = (DescriptionAttribute[])_fieldInfo.GetCustomAttributes(
                typeof(DescriptionAttribute), false);

            return _customAttributes != null && _customAttributes.Any()
                       ? _customAttributes[0].Description
                       : myEnum.ToString();
        }

        /// <summary>
        /// Obter o valor Titulo da Annotation Description: [Description("Minha Descrição", "Meu titulo")]
        /// </summary>
        /// <typeparam name="TType">automaticamente obtido com o objeto.</typeparam>
        /// <param name="myEnum">objeto do tipo Enum.</param>
        /// <returns></returns>

        public static string GetTitle<TType>(this TType myEnum)
        {
            var _fieldInfo = myEnum.GetType().GetField(myEnum.ToString());

            var _customAttributes = (DescriptionAttribute[])_fieldInfo.GetCustomAttributes(
                typeof(DescriptionAttribute), false);

            return
                   _customAttributes != null && _customAttributes.Any()
                       ? _customAttributes[0].Title
                       : myEnum.ToString();
        }

        public static bool TryDeserializeJson<TType>(this object objToConvert, out TType resultParsed)
        {
            resultParsed = default;

            try
            {
                if (!(objToConvert is string))
                    objToConvert = objToConvert.ToJson();

                resultParsed = JsonConvert.DeserializeObject<TType>(objToConvert as string);

                return resultParsed != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Converter o objeto atual para JSON.
        /// </summary>
        /// <typeparam name="TType"></typeparam>
        /// <param name="myObject"></param>
        /// <returns></returns>
        public static string ToJson<TType>(this TType myObject)
        {
            return JsonConvert.SerializeObject(myObject, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented,
                PreserveReferencesHandling = PreserveReferencesHandling.None,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            });
        }

        /// <summary>
        /// Obter os erros do ModelState separados por ponto e virgula.
        /// </summary>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public static string GetAllErrorsToString(this ModelStateDictionary modelState)
        {
            IEnumerable<string> _errors = modelState?.Values.SelectMany(x => x?.Errors)
                            ?.Select(x => x?.ErrorMessage);

            return _errors != null && _errors.Any()
                        ? string.Join(" ", _errors)
                        : "Verifique os dados inseridos.";
        }

        #endregion Metodos Publicos
    }
}