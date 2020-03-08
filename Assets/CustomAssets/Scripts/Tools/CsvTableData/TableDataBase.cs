using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Singleton;
using MyTools.Extensions.Common;
using MyTools.Extensions.String;
using System.IO;

namespace MyTools.Data
{
    using LW = LumenWorks.Framework.IO.Csv;

    public abstract class TableDataBase<TMe> : Singleton<TMe> where TMe : TableDataBase<TMe>, new()
    {
        protected abstract string Path { get; }

        protected abstract void ReadHeaders(List<string> fields);
        protected abstract void ReadRow(List<string> fields);

        public TableDataBase()
        {
            var str = Resources.Load<TextAsset>(Path);
            if (str == null) return;
            using (var csv = new LW.CsvReader(new StringReader(str.text), true))
            {
                int columnsCount = csv.FieldCount;
                List<string> fields = new List<string>(columnsCount);
                fields.AddRange(csv.GetFieldHeaders());
                ReadHeaders(fields);
                while (csv.ReadNextRecord())
                {
                    fields.Clear();
                    for (int i = 0; i < columnsCount; ++i) fields.Add(csv[i]);
                    ReadRow(fields);
                }
            }
        }
    }
}
