using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BP2Projekt.Util
{
    class GodinaValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            DateTime godina;

            if (!DateTime.TryParseExact(value as string, "yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out godina))
                return new ValidationResult(false, "Godina mora biti u YYYY formatu!");

            return new ValidationResult(true, null);
        }
    }
}
