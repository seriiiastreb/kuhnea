using System;
using System.Text;
using System.Security.Cryptography;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace K.Crypt
{
    public class Utils
    {
        public static bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

        public static DateTime ToDateTime(string inputString, string format)
        {
            DateTime result = DateTime.MinValue;

            if (!inputString.Equals(String.Empty))
            {
                DateTime.TryParseExact(inputString, format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out result);
            }

            return result;
        }

        public static decimal MyDecimalParce(string value)
        {
            decimal defaultValue = 0m;

            decimal result = 0;
            if (!string.IsNullOrEmpty(value))
            {
                string output;

                // check if last seperator==groupSeperator
                string groupSep = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator;
                if (value.LastIndexOf(groupSep) + 4 == value.Count())
                {
                    bool tryParse = decimal.TryParse(value, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CurrentCulture, out result);
                    result = tryParse ? result : defaultValue;
                }
                else
                {
                    // unify string (no spaces, only . )     
                    output = value.Trim().Replace(" ", string.Empty).Replace(",", ".");

                    // split it on points     
                    string[] split = output.Split('.');

                    if (split.Count() > 1)
                    {
                        // take all parts except last         
                        output = string.Join(string.Empty, split.Take(split.Count() - 1).ToArray());

                        // combine token parts with last part         
                        output = string.Format("{0}.{1}", output, split.Last());
                    }
                    // parse double invariant     
                    result = decimal.Parse(output, System.Globalization.CultureInfo.InvariantCulture);
                }
            }
            return result;
        }

        public static string ReplaceAcronim(string sourceText)
        {
            //N instead Ñ
            string result = sourceText;

            result = result.Replace('Ñ', 'N');
            result = result.Replace('Ñ', 'N');
            result = result.Replace('í', 'i');
            result = result.Replace('í', 'i');
            result = result.Replace('ª', 'a');
            result = result.Replace('º', ' ');
            result = result.Replace('?', ' ');

            result = result.Replace('Á', 'A');
            result = result.Replace('Â', 'A');
            result = result.Replace('Ã', 'A');
            result = result.Replace('À', 'A');
            result = result.Replace('Ç', 'C');
            result = result.Replace('É', 'E');
            result = result.Replace('È', 'E');
            result = result.Replace('Ê', 'E');
            result = result.Replace('Í', 'I');
            result = result.Replace('Ï', 'I');
            result = result.Replace('Ó', 'O');
            result = result.Replace('Ö', 'O');
            result = result.Replace('Ô', 'O');
            result = result.Replace('Õ', 'O');
            result = result.Replace('Ú', 'U');
            result = result.Replace('Ü', 'U');
            result = result.Replace('ô', 'o');
            result = result.Replace('ö', 'o');
            result = result.Replace('ó', 'o');
            result = result.Replace('õ', 'o');
            result = result.Replace('í', 'i');
            result = result.Replace('ê', 'e');
            result = result.Replace('é', 'e');
            result = result.Replace('è', 'e');
            result = result.Replace('ç', 'c');
            result = result.Replace('à', 'a');
            result = result.Replace('â', 'a');
            result = result.Replace('á', 'a');
            result = result.Replace('ã', 'a');
            result = result.Replace('ü', 'u');
            result = result.Replace('ú', 'u');
            result = result.Replace('ï', 'i');
            result = result.Replace('ø', 'o');

            /////// literele romana
            result = result.Replace("?", "");
            result = result.Replace('ț', 't');
            result = result.Replace('Ț', 'T');
            result = result.Replace('ă', 'a');
            result = result.Replace('Ă', 'A');
            result = result.Replace('â', 'a');
            result = result.Replace('Â', 'A');
            result = result.Replace('ș', 's');
            result = result.Replace('Ș', 'S');
            result = result.Replace('Î', 'I');
            result = result.Replace('î', 'i');

            return result.Trim();
        }

        public static char GetNexLetter(char letter, int nextPozition)
        {
            char result = ' ';

            if (letter == 'z')
                result = 'a';
            else if (letter == 'Z')
                result = 'A';
            else
                result = (char)(((int)letter) + nextPozition);

            return result;
        }

        public static DateTime ConvertStringDateToDateTime(string inputString)
        {
            DateTime result = DateTime.MinValue;

            string[] datestring = inputString.Split('.');

            if (datestring.Length == 3)
            {
                int day, month, year = 0;
                int.TryParse(datestring[0], out day);
                int.TryParse(datestring[1], out month);
                int.TryParse(datestring[2], out year);

                result = new DateTime(year, month, day);
            }

            return result;
        }

        public static string ConvertDataTableToString(DataTable sourceDataTable, string columnName)
        {
            string resultString = string.Empty;

            if (sourceDataTable != null && sourceDataTable.Rows.Count > 0)
            {
                for (int i = 0; i < sourceDataTable.Rows.Count; i++)
                {
                    if (i > 0)
                    {
                        resultString += ", ";
                    }
                    resultString += "'" + sourceDataTable.Rows[i][columnName].ToString() + "'";
                }
            }
            else
            {
                resultString = string.Empty;
            }

            return resultString;
        }

        public static List<string> ConvertDataTableToList(DataTable mSelected, string columnName)
        {
            List<string> resultList = new List<string>();

            if (mSelected != null && mSelected.Rows.Count > 0)
            {
                for (int i = 0; i < mSelected.Rows.Count; i++)
                {
                    resultList.Add(mSelected.Rows[i][columnName].ToString());
                }
            }

            return resultList;
        }

        public static string ConvertListToString(List<string> mSelected)
        {
            string resultString = string.Empty;

            if (mSelected != null && mSelected.Count > 0)
            {
                for (int i = 0; i < mSelected.Count; i++)
                {
                    if (i > 0)
                    {
                        resultString += ", ";
                    }
                    resultString += "'" + mSelected[i] + "'";
                }
            }

            return resultString;
        }

        public static string ConvertListToString(List<int> mSelected)
        {
            string resultString = string.Empty;

            if (mSelected != null && mSelected.Count > 0)
            {
                for (int i = 0; i < mSelected.Count; i++)
                {
                    if (i > 0)
                    {
                        resultString += ", ";
                    }
                    resultString += mSelected[i];
                }
            }

            return resultString;
        }

        public static string GetNameOfMonthBymonthNumber(int languageID, int montNumber)
        {
            string result = string.Empty;

            switch (languageID)
            {
                case (int)K.Constants.Classifiers.Romanian_Language:

                    switch (montNumber)
                    {
                        case 1:
                            result = "Ianuarie";
                            break;
                        case 2:
                            result = "Februarie";
                            break;
                        case 3:
                            result = "Martie";
                            break;
                        case 4:
                            result = "Aprilie";
                            break;
                        case 5:
                            result = "Mai";
                            break;
                        case 6:
                            result = "Iunie";
                            break;
                        case 7:
                            result = "Iulie";
                            break;
                        case 8:
                            result = "August";
                            break;
                        case 9:
                            result = "Septembrie";
                            break;
                        case 10:
                            result = "Octombrie";
                            break;
                        case 11:
                            result = "Noiembrie";
                            break;
                        case 12:
                            result = "Decembrie";
                            break;
                    }

                    break;

                case (int)K.Constants.Classifiers.Russian_Language:

                    switch (montNumber)
                    {
                        case 1:
                            result = "Январь";
                            break;
                        case 2:
                            result = "Февраль";
                            break;
                        case 3:
                            result = "Март";
                            break;
                        case 4:
                            result = "Апрель";
                            break;
                        case 5:
                            result = "Май";
                            break;
                        case 6:
                            result = "Июнь";
                            break;
                        case 7:
                            result = "Июль";
                            break;
                        case 8:
                            result = "Август";
                            break;
                        case 9:
                            result = "Сентябрь";
                            break;
                        case 10:
                            result = "Октябрь";
                            break;
                        case 11:
                            result = "Ноябрь";
                            break;
                        case 12:
                            result = "Декабрь";
                            break;
                    }

                    break;

                case (int)K.Constants.Classifiers.English_Language:

                    switch (montNumber)
                    {
                        case 1:
                            result = "January";
                            break;
                        case 2:
                            result = "February";
                            break;
                        case 3:
                            result = "March";
                            break;
                        case 4:
                            result = "April";
                            break;
                        case 5:
                            result = "May";
                            break;
                        case 6:
                            result = "June";
                            break;
                        case 7:
                            result = "July";
                            break;
                        case 8:
                            result = "August";
                            break;
                        case 9:
                            result = "September";
                            break;
                        case 10:
                            result = "October";
                            break;
                        case 11:
                            result = "November";
                            break;
                        case 12:
                            result = "December";
                            break;
                    }

                    break;
            }

            return result;
        }

        public static string GetDateTimeInSpecialFormat(DateTime inputDate)
        {
            string result = string.Empty;

            if (inputDate != null && !inputDate.Equals(DateTime.MinValue))
            {
                int inputData_Day = inputDate.Day;
                int inputData_Month = inputDate.Month;
                int inputData_Year = inputDate.Year;

                result = " " + inputData_Day.ToString() + " " + GetNameOfMonthBymonthNumber((int)K.Constants.Classifiers.Romanian_Language, inputData_Month) + " " + inputData_Year.ToString();
            }

            return result;
        }

        public static string GetDateTimeInFullWordsFormat(DateTime inputDate)
        {
            string result = string.Empty;

            if (inputDate != null && !inputDate.Equals(DateTime.MinValue))
            {
                int inputData_Day = inputDate.Day;
                int inputData_Month = inputDate.Month;
                int inputData_Year = inputDate.Year;

                string dayWordString = Utils.GetSumInWord(inputData_Day, (int)K.Constants.Classifiers.Romanian_Language, (int)K.Constants.NumberWordMode.SimpleNumber);
                string yearWordString = Utils.GetSumInWord(inputData_Year, (int)K.Constants.Classifiers.Romanian_Language, (int)K.Constants.NumberWordMode.SimpleNumber);

                result = " " + dayWordString + " " + GetNameOfMonthBymonthNumber((int)K.Constants.Classifiers.Romanian_Language, inputData_Month) + " anul " + yearWordString;
            }

            return result;
        }


        public static byte[] ReadFileAsStream(System.IO.Stream input)
        {
            byte[] buffer = new byte[input.Length];
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        //private static string GetNameOfSimpleNumber(int inputNumber, int languageID, bool MaculGen)
        //{
        //    string result = string.Empty;

        //    switch (languageID)
        //    {
        //        case (int)K.Constants.Classifiers.Romanian_Language:

        //            switch (inputNumber)
        //            {
        //                case 0:
        //                    result = "zero";
        //                    break;

        //                case 1:
        //                    if (MaculGen)
        //                        result = "unu";
        //                    else
        //                        result = "una";
        //                    break;

        //                case 2:
        //                    if(MaculGen)
        //                        result = "doi";
        //                    else
        //                        result = "două";
        //                    break;

        //                case 3:
        //                    result = "trei";
        //                    break;

        //                case 4:
        //                    result = "patru";
        //                    break;

        //                case 5:
        //                    result = "cinci";
        //                    break;

        //                case 6:
        //                    result = "șase";
        //                    break;

        //                case 7:
        //                    result = "șapte";
        //                    break;

        //                case 8:
        //                    result = "opt";
        //                    break;

        //                case 9:
        //                    result = "nouă";
        //                    break;

        //                case 10:
        //                    result = "zece";
        //                    break;

        //                case 11:
        //                    result = "unsprezece";
        //                    break;

        //                case 12:
        //                    result = "douăsprezece";
        //                    break;

        //                case 13:
        //                    result = "treisprezece";
        //                    break;

        //                case 14:
        //                    result = "paisprezece";
        //                    break;

        //                case 15:
        //                    result = "cincisprezece";
        //                    break;

        //                case 16:
        //                    result = "șaisprezece";
        //                    break;

        //                case 17:
        //                    result = "șaptesprezece";
        //                    break;

        //                case 18:
        //                    result = "optsprezece";
        //                    break;

        //                case 19:
        //                    result = "nouăsprezece";
        //                    break;

        //                case 20:
        //                    result = "douăzeci";
        //                    break;  

        //                case 30:
        //                    result = "treizeci";
        //                    break;

        //                case 40:
        //                    result = "patruzeci";
        //                    break;

        //                case 50:
        //                    result = "cincizeci";
        //                    break;

        //                case 60:
        //                    result = "șaizeci";
        //                    break;

        //                case 70:
        //                    result = "șaptezeci";
        //                    break;

        //                case 80:
        //                    result = "optzeci";
        //                    break;

        //                case 90:
        //                    result = "nouăzeci";
        //                    break;
        //            }

        //            break;


        //        case (int)K.Constants.Classifiers.Russian_Language:

        //            switch (inputNumber)
        //            {
        //                case 0:
        //                    result = "ноль";
        //                    break;

        //                case 1:
        //                    if (MaculGen)
        //                        result = "один";
        //                    else
        //                        result = "одна";
        //                    break;

        //                case 2:
        //                    if (MaculGen)
        //                        result = "два";
        //                    else
        //                        result = "две";
        //                    break;

        //                case 3:
        //                    result = "три";
        //                    break;

        //                case 4:
        //                    result = "четыре";
        //                    break;

        //                case 5:
        //                    result = "пять";
        //                    break;

        //                case 6:
        //                    result = "шесть";
        //                    break;

        //                case 7:
        //                    result = "семь";
        //                    break;

        //                case 8:
        //                    result = "восемь";
        //                    break;

        //                case 9:
        //                    result = "девять";
        //                    break;

        //                case 10:
        //                    result = "десять";
        //                    break;

        //                case 11:
        //                    result = "одиннадцать";
        //                    break;

        //                case 12:
        //                    result = "двенадцать";
        //                    break;

        //                case 13:
        //                    result = "тринадцать";
        //                    break;

        //                case 14:
        //                    result = "четырнадцать";
        //                    break;

        //                case 15:
        //                    result = "пятнадцать";
        //                    break;

        //                case 16:
        //                    result = "шестнадцать";
        //                    break;

        //                case 17:
        //                    result = "семнадцать";
        //                    break;

        //                case 18:
        //                    result = "восемнадцать";
        //                    break;

        //                case 19:
        //                    result = "девятнадцать";
        //                    break;

        //                case 20:
        //                    result = "двадцать";
        //                    break;

        //                case 30:
        //                    result = "тридцать";
        //                    break;

        //                case 40:
        //                    result = "сорок";
        //                    break;

        //                case 50:
        //                    result = "пятьдесят";
        //                    break;

        //                case 60:
        //                    result = "шестьдесят";
        //                    break;

        //                case 70:
        //                    result = "семьдесят";
        //                    break;

        //                case 80:
        //                    result = "восемьдесят";
        //                    break;

        //                case 90:
        //                    result = "девяносто";
        //                    break;  
        //            }

        //            break;

        //        case (int)K.Constants.Classifiers.English_Language:

        //            switch (inputNumber)
        //            {
        //                case 0:
        //                    result = "zero";
        //                    break;

        //                case 1:
        //                    result = "one";
        //                    break;

        //                case 2:
        //                    result = "two";
        //                    break;

        //                case 3:
        //                    result = "three";
        //                    break;

        //                case 4:
        //                    result = "four";
        //                    break;

        //                case 5:
        //                    result = "five";
        //                    break;

        //                case 6:
        //                    result = "six";
        //                    break;

        //                case 7:
        //                    result = "seven";
        //                    break;

        //                case 8:
        //                    result = "eight";
        //                    break;

        //                case 9:
        //                    result = "nine";
        //                    break;

        //                case 10:
        //                    result = "ten";
        //                    break;

        //                case 11:
        //                    result = "eleven";
        //                    break;

        //                case 12:
        //                    result = "twelve";
        //                    break;

        //                case 13:
        //                    result = "thirteen";
        //                    break;

        //                case 14:
        //                    result = "fourteen";
        //                    break;

        //                case 15:
        //                    result = "fifteen";
        //                    break;

        //                case 16:
        //                    result = "sixteen";
        //                    break;

        //                case 17:
        //                    result = "seventeen";
        //                    break;

        //                case 18:
        //                    result = "eighteen";
        //                    break;

        //                case 19:
        //                    result = "nineteen";
        //                    break;

        //                case 20:
        //                    result = "twenty";
        //                    break;

        //                case 30:
        //                    result = "thirty";
        //                    break;

        //                case 40:
        //                    result = "Forty";
        //                    break;

        //                case 50:
        //                    result = "fifty";
        //                    break;

        //                case 60:
        //                    result = "sixty";
        //                    break;

        //                case 70:
        //                    result = "seventy";
        //                    break;

        //                case 80:
        //                    result = "eighty";
        //                    break;

        //                case 90:
        //                    result = "ninety";
        //                    break;
        //            }

        //            break;
        //    }

        //    return result;
        //}

        //private static string GetErrorMessage(int languageID)
        //{
        //    string result = string.Empty;

        //    switch (languageID)
        //    {
        //        case (int)K.Constants.Classifiers.Romanian_Language:
        //            result = "Numarul introdus este prea mare";
        //            break;

        //        case (int)K.Constants.Classifiers.Russian_Language:
        //            result = "Введенный номер слишком большой";
        //            break;

        //        case (int)K.Constants.Classifiers.English_Language:
        //            result = "The number entered is too large";
        //            break;
        //    }

        //    return result;
        //}

        //private static string GetAndWord(int languageID)
        //{
        //    string result = string.Empty;

        //    switch (languageID)
        //    {
        //        case (int)K.Constants.Classifiers.Romanian_Language:
        //            result = "și";
        //            break;

        //        case (int)K.Constants.Classifiers.Russian_Language:
        //            result = "";
        //            break;

        //        case (int)K.Constants.Classifiers.English_Language:
        //            result = "";
        //            break;
        //    }

        //    return result;
        //}


        private static string GetValutaWord(int languageID)
        {
            string valutaWord = string.Empty;

            switch (languageID)
            {
                case (int)K.Constants.Classifiers.Romanian_Language:
                    valutaWord = "lei";
                    break;

                case (int)K.Constants.Classifiers.Russian_Language:
                    valutaWord = "рублей";
                    break;

            }

            return valutaWord;
        }

        private static string GetValutaWord(int languageID, int amount)
        {
            string valutaWord = string.Empty;

            switch (languageID)
            {
                case (int)K.Constants.Classifiers.Romanian_Language:
                    switch (amount)
                    {
                        default:
                            valutaWord = "lei";
                            break;
                    }
                    break;

                case (int)K.Constants.Classifiers.Russian_Language:
                    switch (amount)
                    {
                        case 1:
                            valutaWord = "рубль";
                            break;
                        case 2:
                            valutaWord = "рубля";
                            break;
                        case 3:
                            valutaWord = "рубля";
                            break;
                        case 4:
                            valutaWord = "рубля";
                            break;
                        default:
                            valutaWord = "рублей";
                            break;
                    }
                    break;
            }

            return valutaWord;
        }

        private static string GetValutaCentWord(int languageID)
        {
            string valutaCentWord = string.Empty;

            switch (languageID)
            {
                case (int)K.Constants.Classifiers.Romanian_Language:
                    valutaCentWord = "bani";
                    break;

                case (int)K.Constants.Classifiers.Russian_Language:
                    valutaCentWord = "копеек";
                    break;
            }

            return valutaCentWord;
        }

        private static string GetWordsAfterPoint(int languageID, int dupaVirgula)
        {
            string result = string.Empty;

            string valutaWord = GetValutaWord(languageID);
            string valutaCentWord = GetValutaCentWord(languageID);

            result = valutaWord + " " + (dupaVirgula == 0 ? "00" : (dupaVirgula < 10 ? "0" + dupaVirgula.ToString() : dupaVirgula.ToString())) + " " + valutaCentWord;

            return result;
        }

        #region Romanian Translate

        private const int ZeciKeyWord = 2;
        private const int SuteKeyWord = 3;
        private const int MiiKeyWord = 4;
        private const int MilioaneKeyWord = 7;
        private const int MiliardeKeyWord = 10;

        private static string GetSuteMiiKeyWord_ROM(int suteMiiKeyNumber, bool plural)
        {
            string result = string.Empty;

            switch (suteMiiKeyNumber)
            {
                case 2:
                    result = "zeci";
                    break;

                case 3:
                    if (plural)
                        result = "sute";
                    else
                        result = "sută";
                    break;

                case 4:
                    if (plural)
                        result = "mii";
                    else
                        result = "mie";
                    break;

                case 5:
                    result = "mii";
                    break;

                case 6:
                    result = "mii";
                    break;

                case 7:
                    if (plural)
                        result = "milioane";
                    else
                        result = "milion";
                    break;

                case 8:
                    result = "milioane";
                    break;

                case 9:
                    result = "milioane";
                    break;

                case 10:
                    if (plural)
                        result = "miliarde";
                    else
                        result = "miliard";
                    break;

                case 11:
                    result = "miliarde";
                    break;

                case 12:
                    result = "miliarde";
                    break;
            }

            return result;
        }

        private static string GeSimpleNumberWord_ROM(int inputNumber, bool MaculGen)
        {
            string result = string.Empty;

            switch (inputNumber)
            {
                case 0:
                    result = "zero";
                    break;

                case 1:
                    if (MaculGen)
                        result = "unu";
                    else
                        result = "una";
                    break;

                case 2:
                    if (MaculGen)
                        result = "doi";
                    else
                        result = "două";
                    break;

                case 3:
                    result = "trei";
                    break;

                case 4:
                    result = "patru";
                    break;

                case 5:
                    result = "cinci";
                    break;

                case 6:
                    result = "șase";
                    break;

                case 7:
                    result = "șapte";
                    break;

                case 8:
                    result = "opt";
                    break;

                case 9:
                    result = "nouă";
                    break;

                case 10:
                    result = "zece";
                    break;

                case 11:
                    result = "unsprezece";
                    break;

                case 12:
                    result = "douăsprezece";
                    break;

                case 13:
                    result = "treisprezece";
                    break;

                case 14:
                    result = "paisprezece";
                    break;

                case 15:
                    result = "cincisprezece";
                    break;

                case 16:
                    result = "șaisprezece";
                    break;

                case 17:
                    result = "șaptesprezece";
                    break;

                case 18:
                    result = "optsprezece";
                    break;

                case 19:
                    result = "nouăsprezece";
                    break;
            }

            return result;
        }

        private static string GetSumInWords_ROM(int pinaLaVirgula)
        {
            string result = string.Empty;

            int lenghtOfNumber = pinaLaVirgula.ToString().Length;

            bool genMasculin = true;
            bool genFemenin = false;

            bool plural = true;
            bool singular = false;

            switch (lenghtOfNumber)
            {
                case 1: // 5
                    result = GeSimpleNumberWord_ROM(pinaLaVirgula, genMasculin);
                    break;

                case 2: // 50
                    {
                        if (pinaLaVirgula < 20)
                        {
                            result = GeSimpleNumberWord_ROM(pinaLaVirgula, genMasculin);
                        }
                        else
                        {
                            int firstNumber = pinaLaVirgula / 10;
                            int lastNumber = pinaLaVirgula % 10;

                            result = GeSimpleNumberWord_ROM(firstNumber, genFemenin) + GetSuteMiiKeyWord_ROM(ZeciKeyWord, plural) + (lastNumber > 0 ? " și " + GeSimpleNumberWord_ROM(lastNumber, genMasculin) : string.Empty);
                        }
                    }
                    break;

                case 3: // 500
                    {
                        int suteNumber = pinaLaVirgula / 100;
                        int zeciNumber = pinaLaVirgula % 100;

                        result = GeSimpleNumberWord_ROM(suteNumber, genFemenin) + " " + (suteNumber == 1 ? GetSuteMiiKeyWord_ROM(SuteKeyWord, singular) : GetSuteMiiKeyWord_ROM(SuteKeyWord, plural)) + " " + GetSumInWords_ROM(zeciNumber);
                    }
                    break;

                case 4: // 5 000                
                    {
                        int miiNumber = pinaLaVirgula / 1000;
                        int suteNumber = pinaLaVirgula % 1000;

                        result = GeSimpleNumberWord_ROM(miiNumber, genFemenin) + " " + (miiNumber == 1 ? GetSuteMiiKeyWord_ROM(MiiKeyWord, singular) : GetSuteMiiKeyWord_ROM(MiiKeyWord, plural)) + " " + GetSumInWords_ROM(suteNumber);
                    }
                    break;

                case 5: // 50 000
                case 6: // 500 000
                    {
                        int miiNumber = pinaLaVirgula / 1000;
                        int suteNumber = pinaLaVirgula % 1000;

                        result = GetSumInWords_ROM(miiNumber) + " " + GetSuteMiiKeyWord_ROM(MiiKeyWord, plural) + " " + GetSumInWords_ROM(suteNumber);
                    }
                    break;

                case 7: // 5 000 000
                    {
                        int milionNumber = pinaLaVirgula / 1000000;
                        int miiiNumber = pinaLaVirgula % 1000000;

                        result = GeSimpleNumberWord_ROM(milionNumber, genFemenin) + " " + (milionNumber == 1 ? GetSuteMiiKeyWord_ROM(MilioaneKeyWord, singular) : GetSuteMiiKeyWord_ROM(MilioaneKeyWord, plural)) + " " + GetSumInWords_ROM(miiiNumber);
                    }
                    break;

                case 8: // 50 000 000
                case 9: // 500 000 000
                    {
                        int milionNumber = pinaLaVirgula / 1000000;
                        int miiiNumber = pinaLaVirgula % 1000000;

                        result = GetSumInWords_ROM(milionNumber) + " " + GetSuteMiiKeyWord_ROM(MilioaneKeyWord, plural) + " " + GetSumInWords_ROM(miiiNumber);
                    }
                    break;

                case 10: // 5 000 000 000
                    {
                        int milionNumber = pinaLaVirgula / 1000000000;
                        int miiiNumber = pinaLaVirgula % 1000000000;

                        result = GeSimpleNumberWord_ROM(milionNumber, genFemenin) + " " + (milionNumber == 1 ? GetSuteMiiKeyWord_ROM(MiliardeKeyWord, singular) : GetSuteMiiKeyWord_ROM(MiliardeKeyWord, plural)) + " " + GetSumInWords_ROM(miiiNumber);
                    }
                    break;

                case 11: // 50 000 000 000
                case 12: // 500 000 000 000
                    {
                        int milionNumber = pinaLaVirgula / 1000000000;
                        int miiiNumber = pinaLaVirgula % 1000000000;

                        result = GetSumInWords_ROM(milionNumber) + " " + GetSuteMiiKeyWord_ROM(MiliardeKeyWord, plural) + " " + GetSumInWords_ROM(miiiNumber);
                    }
                    break;

                default:
                    break;
            }

            return result;
        }

        #endregion Romanian Translate

        public static string GetSumInWord(decimal number, int languageID, int numberWordMode)
        {
            string result = string.Empty;

            switch (languageID)
            {
                case (int)K.Constants.Classifiers.Romanian_Language:

                    int pinaLaVirgula = (int)(Math.Abs(number));
                    int dupaVirgula = (int)((Math.Abs(number) - pinaLaVirgula) * 100);

                    switch (numberWordMode)
                    {
                        case (int)K.Constants.NumberWordMode.Money:
                            result = GetSumInWords_ROM(pinaLaVirgula) + " " + GetWordsAfterPoint(languageID, dupaVirgula);
                            break;

                        case (int)K.Constants.NumberWordMode.SimpleNumber:
                            result = GetSumInWords_ROM(pinaLaVirgula) + " intreg , " + (dupaVirgula == 0 ? "00" : (dupaVirgula < 10 ? "0" + dupaVirgula.ToString() : dupaVirgula.ToString()));
                            break;

                        case (int)K.Constants.NumberWordMode.Percent:
                            result = GetSumInWords_ROM(pinaLaVirgula) + " intreg , " + (dupaVirgula == 0 ? "00" : (dupaVirgula < 10 ? "0" + dupaVirgula.ToString() : dupaVirgula.ToString())) + " procente";
                            break;
                    }

                    break;

                case (int)K.Constants.Classifiers.Russian_Language:

                    break;

                case (int)K.Constants.Classifiers.English_Language:

                    break;
            }

            return result;
        }

        public static string GetValutaNameInWords(int languageID, int valutaID, bool infinitiv)
        {
            string result = string.Empty;

            switch (languageID)
            {
                case (int)K.Constants.Classifiers.Romanian_Language:
                    switch (valutaID)
                    {
                        case (int)K.Constants.CurrencyList.MDL:
                            result = infinitiv ? "lei moldovenesti" : "leului moldovenesc";
                            break;
                        case (int)K.Constants.CurrencyList.USD:
                            result = infinitiv ? "dolari SUA" : "dolarului SUA";
                            break;
                        case (int)K.Constants.CurrencyList.EURO:
                            result = infinitiv ? "EURO" : "EURO";
                            break;
                    }
                    break;

                case (int)K.Constants.Classifiers.Russian_Language:
                    switch (valutaID)
                    {
                        case (int)K.Constants.CurrencyList.MDL:
                            break;
                        case (int)K.Constants.CurrencyList.USD:
                            break;
                        case (int)K.Constants.CurrencyList.EURO:
                            break;
                    }
                    break;

                case (int)K.Constants.Classifiers.English_Language:
                    switch (valutaID)
                    {
                        case (int)K.Constants.CurrencyList.MDL:
                            break;
                        case (int)K.Constants.CurrencyList.USD:
                            break;
                        case (int)K.Constants.CurrencyList.EURO:
                            break;
                    }
                    break;
            }

            return result;
        }

        public static string GetCurrencyChangeValutaSimbols(int firstValuta, int secondValuta)
        {
            string result = string.Empty;

            if (firstValuta == (int)K.Constants.CurrencyList.MDL && secondValuta == (int)K.Constants.CurrencyList.USD)
            {
                result = "MDL/USD";
            }
            else
                if (firstValuta == (int)K.Constants.CurrencyList.USD && secondValuta == (int)K.Constants.CurrencyList.MDL)
                {
                    result = "USD/MDL";
                }
                else
                    if (firstValuta == (int)K.Constants.CurrencyList.MDL && secondValuta == (int)K.Constants.CurrencyList.EURO)
                    {
                        result = "MDL/EURO";
                    }
                    else
                        if (firstValuta == (int)K.Constants.CurrencyList.EURO && secondValuta == (int)K.Constants.CurrencyList.MDL)
                        { result = "EURO/MDL"; }

            return result;
        }

        public static DataTable AddTotalsToTable(DataTable sourceTable, string columnToInsertTotalWord, string totalWord, List<string> columnsForCalculateTotals)
        {
            if (sourceTable.Columns.Contains(columnToInsertTotalWord))
            {
                try
                {
                    double[] totalsList = new double[columnsForCalculateTotals.Count];

                    for (int i = 0; i < sourceTable.Rows.Count; i++)
                    {
                        for (int j = 0; j < columnsForCalculateTotals.Count; j++)
                        {
                            if (sourceTable.Columns.Contains(columnsForCalculateTotals[j]))
                            {
                                double tempNumber = 0;
                                double.TryParse(sourceTable.Rows[i][columnsForCalculateTotals[j]].ToString(), out tempNumber);

                                double temTotal = totalsList[j] + tempNumber;
                                totalsList[j] = temTotal;
                            }
                        }
                    }

                    sourceTable.Rows.Add();
                    sourceTable.Rows.Add();

                    for (int j = 0; j < columnsForCalculateTotals.Count; j++)
                    {
                        sourceTable.Rows[sourceTable.Rows.Count - 1][columnsForCalculateTotals[j]] = totalsList[j];
                    }
                }
                catch { }
            }

            return sourceTable;
        }
    }
}
