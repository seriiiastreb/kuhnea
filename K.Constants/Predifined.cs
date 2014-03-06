using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K.Constants
{
    public enum ClassifierTypes
    {
        Undefined = 0,
        SystemRoleType = 1,
        PasswordStatus = 2,
        CountryList = 3,
        GenderList = 4,
        LanguageList = 5,
        SystemUserRecordStatus = 6
    }

    public enum Classifiers
    {
        Undefined = 0,
        Romanian_Language = 1,
        Russian_Language = 2,
        English_Language = 3,
        PasswordStatusActive = 4,

        UserRecord_Active = 8,
        UserRecord_Blocked = 9,
        UserRecord__NotActivated = 10
    }

    public enum CurrencyList
    {
        MDL = 1,
        USD = 2,
        EURO = 3
    }

    public enum NumberWordMode
    {
        Money = 1,
        Percent = 2,
        SimpleNumber = 3
    }

}
