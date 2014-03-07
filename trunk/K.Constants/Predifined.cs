using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K.Constants
{
    public enum UserLanguage
    {
        English = 1,
        Romanian = 2, 
        Russian = 3,
        French = 4,
        German = 5
    }

    public enum ClassifierTypes
    {
        Undefined = 0,
        PasswordStatus = 1,
        CountryList = 2,
        GenderList = 3,
        UserRecordStatus = 4
    }

    public enum Classifiers
    {
        Undefined = 0,
        PasswordStatusActive = 1,
        PasswordStatusNeedChange = 2,

        UserRecord_Active = 8,
        UserRecord_Blocked = 9,
        UserRecord_NotActivated = 10
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
