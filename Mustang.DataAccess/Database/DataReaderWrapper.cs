using System;
using System.Data;

namespace Mustang.DataAccess
{
    public abstract class DataReaderWrapper : MarshalByRefObject, IDataReader
    {
        private readonly IDataReader innerReader;

        protected DataReaderWrapper(IDataReader innerReader)
        {
            this.innerReader = innerReader;
        }

        public IDataReader InnerReader { get { return innerReader; } }

        public virtual int FieldCount => innerReader.FieldCount;

        public virtual int Depth => innerReader.Depth;

        public virtual bool IsClosed => innerReader.IsClosed;

        public virtual int RecordsAffected => innerReader.RecordsAffected;

        public virtual void Close()
        {
            if (!innerReader.IsClosed)
            {
                innerReader.Close();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!innerReader.IsClosed)
                {
                    innerReader.Dispose();
                }
            }
        }

        public virtual string GetName(int i)
        {
            return innerReader.GetName(i);
        }

        public virtual string GetDataTypeName(int i)
        {
            return innerReader.GetDataTypeName(i);
        }

        public virtual Type GetFieldType(int i)
        {
            return innerReader.GetFieldType(i);
        }

        public virtual object GetValue(int i)
        {
            return innerReader.GetValue(i);
        }

        public virtual int GetValues(object[] values)
        {
            return innerReader.GetValues(values);
        }

        public virtual int GetOrdinal(string name)
        {
            return innerReader.GetOrdinal(name);
        }

        public virtual bool GetBoolean(int i)
        {
            return innerReader.GetBoolean(i);
        }

        public virtual byte GetByte(int i)
        {
            return innerReader.GetByte(i);
        }

        public virtual long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            return innerReader.GetBytes(i, fieldOffset, buffer, bufferoffset, length);
        }

        public virtual char GetChar(int i)
        {
            return innerReader.GetChar(i);
        }

        public virtual long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            return innerReader.GetChars(i, fieldoffset, buffer, bufferoffset, length);
        }

        public virtual Guid GetGuid(int i)
        {
            return innerReader.GetGuid(i);
        }

        public virtual short GetInt16(int i)
        {
            return innerReader.GetInt16(i);
        }

        public virtual int GetInt32(int i)
        {
            return innerReader.GetInt32(i);
        }

        public virtual long GetInt64(int i)
        {
            return innerReader.GetInt64(i);
        }

        public virtual float GetFloat(int i)
        {
            return innerReader.GetFloat(i);
        }

        public virtual double GetDouble(int i)
        {
            return innerReader.GetDouble(i);
        }

        public virtual string GetString(int i)
        {
            return innerReader.GetString(i);
        }

        public virtual decimal GetDecimal(int i)
        {
            return innerReader.GetDecimal(i);
        }

        public virtual DateTime GetDateTime(int i)
        {
            return innerReader.GetDateTime(i);
        }

        public virtual IDataReader GetData(int i)
        {
            return innerReader.GetData(i);
        }

        public virtual bool IsDBNull(int i)
        {
            return innerReader.IsDBNull(i);
        }

        object IDataRecord.this[int i] => innerReader[i];

        object IDataRecord.this[string name] => innerReader[name];

        public virtual DataTable GetSchemaTable()
        {
            return innerReader.GetSchemaTable();
        }

        public virtual bool NextResult()
        {
            return innerReader.NextResult();
        }

        public virtual bool Read()
        {
            return innerReader.Read();
        }
    }
}
