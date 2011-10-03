using System;
using System.Data;
using Ngol.Hytek.Interfaces;
using NHibernate;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;

namespace Ngol.XcAnalyze.Persistence.Types
{
    /// <summary>
    /// Mapping between MySQL enumeration and <see cref="Gender" /> enumeration.
    /// </summary>
    public class GenderType : IUserType
    {
        #region IUserType implementation

        /// <inheritdoc />
        public bool IsMutable
        {
            get { return false; }
        }

        /// <inheritdoc />
        public Type ReturnedType
        {
            get { return typeof(Gender); }
        }

        /// <inheritdoc />
        public SqlType[] SqlTypes
        {
            get { return new[] { NHibernateUtil.String.SqlType }; }
        }

        /// <inheritdoc />
        public new bool Equals(object x, object y)
        {
            if(ReferenceEquals(x, y))
            {
                return true;
            }
            if(x == null || y == null)
            {
                return false;
            }
            return x.Equals(y);
        }

        /// <inheritdoc />
        public int GetHashCode(object x)
        {
            return x == null ? typeof(Gender).GetHashCode() + 473 : x.GetHashCode();
        }

        /// <inheritdoc />
        public object NullSafeGet(IDataReader reader, string[] names, object owner)
        {
            var obj = NHibernateUtil.String.NullSafeGet(reader, names[0]);
            //Get the object
            if(obj == null)
                return null;
            string genderString = (string)obj;
            if(genderString != "M" && genderString != "F")
                throw new Exception();
            if(genderString == "M")
                return Gender.Male;
            return Gender.Female;
        }

        /// <inheritdoc />
        public void NullSafeSet(IDbCommand command, object value, int index)
        {
            IDataParameter parameter = (IDataParameter)command.Parameters[index];
            if(value == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                Gender gender = (Gender)value;
                parameter.Value = gender == Gender.Male ? "M" : "F";
            }
        }

        /// <inheritdoc />
        public object DeepCopy(object value)
        {
            return value;
        }

        /// <inheritdoc />
        public object Replace(object original, object target, object owner)
        {
            return original;
        }

        /// <inheritdoc />
        public object Assemble(object cached, object owner)
        {
            return cached;
        }

        /// <inheritdoc />
        public object Disassemble(object value)
        {
            return value;
        }

        #endregion
    }
}
