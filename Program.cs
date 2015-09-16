//
// XMLReadWriteTest.cs - C#でXMLの読み書きのメモ
//
using System;
using System.Collections;
using System.Xml.Serialization;
using System.IO;

namespace XMLReadWriteTest
{
    [Serializable]
    public class Item
    {
        [XmlAttribute("Id")]        // XmlAttributeを指定するとattributeとしてシリアライズされる
        public int Id { get; set; } //getter,setterは必ず用意すること

        // 何も指定しないと子elementとしてシリアライズされる
        public string Message { get; set; }

        public Item()
        {
        }

        public Item(int id, String message)
        {
            this.Id = id;
            this.Message = message;
        }

        public override string ToString()
        {
            return string.Format("id={0}, message={1}", this.Id, this.Message);
        }
    }

    [Serializable]
    public class ItemContainer
    {
        // XmlArrayItemを使ってArrayListに入る型を指定する。カンマ区切りで複数指定可能
        [XmlArrayItem(typeof(Item))]
        public ArrayList Items;

        public ItemContainer()
        {
            this.Items = new ArrayList();
        }

        public void dump()
        {
            Console.WriteLine("ItemContainer.Items");
            for (int i = 0; i < Items.Count; i++)
            {
                Console.WriteLine("    {0} : {1}", i, Items[i].ToString());
            }
        }
    }

    class Program
    {
        static string Serialize(ItemContainer item_container)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ItemContainer));

            // ファイルに書き出すときはStreamWriterを使う
            //StreamWriter sw = new StreamWriter("test.xml", false, new System.Text.UTF8Encoding(false));
            //serializer.Serialize(sw, item_container);
            //sw.Close();

            // オブジェクト→文字列
            StringWriter tw = new StringWriter();
            serializer.Serialize(tw, item_container);
            string xml = tw.ToString();

            return xml;
        }

        static ItemContainer Deserialize(string xml)
        {
            ItemContainer item_container;

            XmlSerializer serializer = new XmlSerializer(typeof(ItemContainer));

            // ファイルから読み込むときはStreamReaderを使う
            //StreamReader sr = new StreamReader("test.xml", new System.Text.UTF8Encoding(false));
            //item_container = (ItemContainer)serializer.Deserialize(sr);
            //sr.Close();

            // XML→オブジェクト
            StringReader tr = new StringReader(xml);
            item_container = (ItemContainer)serializer.Deserialize(tr);

            return item_container;
        }

        static void Main(string[] args)
        {
            // 元データ
            ItemContainer item_container = new ItemContainer();
            item_container.Items.Add(new Item(1, "aaa"));
            item_container.Items.Add(new Item(2, "bbb"));
            item_container.Items.Add(new Item(3, "ccc"));
            item_container.dump();

            // オブジェクト→XML
            string xml = Serialize(item_container);
            Console.WriteLine(xml);

            // XML→オブジェクト
            ItemContainer tmp = Deserialize(xml);
            tmp.dump();
        }
    }
}
