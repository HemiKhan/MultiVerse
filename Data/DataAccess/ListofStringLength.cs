using System.ComponentModel.DataAnnotations;

namespace Data.DataAccess
{
    public class ListofEachStringLengthAttribute : ValidationAttribute
    {
        private readonly int _minLength;
        private readonly int _maxLength;
        public ListofEachStringLengthAttribute(int MinLength, int MaxLength)
        {
            this._minLength = MinLength;
            this._maxLength = MaxLength;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            List<string> errorslist = new List<string>();
            if (value is List<string> _listofstring)
            {
                int listnumber = 0;
                foreach (string _string in _listofstring)
                {
                    bool iserroradded = false;
                    if (_string != null)
                    {
                        if (_string?.Length < _minLength && iserroradded == false)
                        {
                            errorslist.Add(ErrorMessage.Replace("{listnumber}", listnumber.ToString()));
                            iserroradded = true;
                            //return new ValidationResult(ErrorMessage.Replace("{listnumber}", listnumber.ToString()));
                        }
                        if (_string?.Length > _maxLength && iserroradded == false)
                        {
                            errorslist.Add(ErrorMessage.Replace("{listnumber}", listnumber.ToString()));
                            //return new ValidationResult(ErrorMessage.Replace("{listnumber}", listnumber.ToString()));
                        }
                    }
                    listnumber++;
                }
                if (errorslist.Count > 0)
                    return new ValidationResult(string.Join("; ", errorslist));

                return ValidationResult.Success;
            }

            return new ValidationResult("Invalid Format");
        }
    }
}
