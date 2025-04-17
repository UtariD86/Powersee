using Core.Helpers.Abstract;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Application.Helpers.Concrete
{
    public class EnumHelper : IEnumHelper
    {
        List<T> IEnumHelper.GetList<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToList();
        }

        List<KeyValuePair<int, string>> IEnumHelper.GetValuesAndNames<T>()
        {
            return Enum.GetValues(typeof(T))
                   .Cast<Enum>()
                   .Select(e => new KeyValuePair<int, string>(Convert.ToInt32(e), e.ToString()))
                   .ToList();
        }

        // ✅ SelectList üretmek için yeni method
        public static SelectList ToSelectList<T>() where T : Enum
        {
            var values = Enum.GetValues(typeof(T))
                .Cast<T>()
                .Select(e => new SelectListItem
                {
                    Value = Convert.ToInt32(e).ToString(),
                    Text = GetDescription(e)
                });

            return new SelectList(values, "Value", "Text");
        }

        private static string GetDescription<T>(T e)
        {
            var field = e.GetType().GetField(e.ToString());
            var attribute = field.GetCustomAttributes(typeof(DisplayAttribute), false)
                .FirstOrDefault() as DisplayAttribute;
            return attribute.GetName() ?? e.ToString();
        }
    }

}
