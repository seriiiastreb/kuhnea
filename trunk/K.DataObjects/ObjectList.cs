using System;
using System.Collections.Generic;
using System.Data;

namespace K.DataObjects
{
    public class User
    {
        int mUserID = 0;
        string mFirstName = string.Empty;
        string mLastName = string.Empty;
        string mLogin = string.Empty;
        string mPassword = string.Empty;
        int mPasswordstatus = 0;
        int mRecordstatus = 0;
        int mLanguage = 0;
        string mEmail = string.Empty;
        bool mIsSysadmin = false;

        public int UserID
        {
            get { return mUserID; }
            set { mUserID = value; }
        }

        public bool IsSysadmin
        {
            get { return mIsSysadmin; }
            set { mIsSysadmin = value; }
        }

        public string FirstName
        {
            get { return mFirstName; }
            set { mFirstName = value; }
        }

        public string LastName
        {
            get { return mLastName; }
            set { mLastName = value; }
        }

        public string Login
        {
            get { return mLogin; }
            set { mLogin = value; }
        }

        public string Password
        {
            get { return mPassword; }
            set { mPassword = value; }
        }

        public int PasswordStatus
        {
            get { return mPasswordstatus; }
            set { mPasswordstatus = value; }
        }

        public int RecordStatus
        {
            get { return mRecordstatus; }
            set { mRecordstatus = value; }
        }

        public int Language
        {
            get { return mLanguage; }
            set { mLanguage = value; }
        }

        public string Email
        {
            get { return mEmail; }
            set { mEmail = value; }
        }

        public User()
        {
        }

        public User(DataRow UserInfoROw)
        {
            if (UserInfoROw != null)
            {
                mUserID = UserInfoROw["userid"] != System.DBNull.Value ? (int)UserInfoROw["userid"] : 0;
                mFirstName = UserInfoROw["firstname"] != System.DBNull.Value ? (string)UserInfoROw["firstname"] : string.Empty;
                mLastName = UserInfoROw["lastname"] != System.DBNull.Value ? (string)UserInfoROw["lastname"] : string.Empty;
                mLogin = UserInfoROw["login"] != System.DBNull.Value ? (string)UserInfoROw["login"] : string.Empty;
                mPassword = UserInfoROw["password"] != System.DBNull.Value ? (string)UserInfoROw["password"] : string.Empty;
                mPasswordstatus = UserInfoROw["Passwordstatus"] != System.DBNull.Value ? (int)UserInfoROw["Passwordstatus"] : 0;
                mRecordstatus = UserInfoROw["Recordstatus"] != System.DBNull.Value ? (int)UserInfoROw["Recordstatus"] : 0;
                mLanguage = UserInfoROw["language"] != System.DBNull.Value ? (int)UserInfoROw["language"] : 0;
                mEmail = UserInfoROw["email"] != System.DBNull.Value ? (string)UserInfoROw["email"] : string.Empty;
                mIsSysadmin = UserInfoROw["issysadmin"] != System.DBNull.Value ? (bool)UserInfoROw["issysadmin"] : false;
            }
        }


    }


}
