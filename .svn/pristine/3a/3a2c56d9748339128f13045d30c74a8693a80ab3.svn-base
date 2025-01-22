using Base.Common;
using Base.Common.Items;
using Common.Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Common
{
    public class ConfigBase
    {
        private XDocument xml = null;

        public HandleState OpenConfig(string XMLPath)
        {
            try
            {
                xml = XDocument.Load(XMLPath);
                return new HandleState();
            }
            catch (Exception ex)
            {
                xml = null;
                return new HandleState(ex);
            }
        }

        public HandleState SaveConfig(string XMLPath)
        {
            try
            {
                if (xml != null)
                    xml.Save(XMLPath);
                return new HandleState();
            }
            catch (Exception ex)
            {
                xml = null;
                return new HandleState(ex);
            }
        }

        public string Get(string ElementPath, string Attribute, string XMLPath)
        {
            try
            {
                if (xml == null)
                    OpenConfig(XMLPath);

                string[] elements = ElementPath.Split('.');
                XElement xe = null;

                for (int i = 0; i < elements.Length; i++)
                {
                    if (xe == null)
                        xe = xml.Element(elements[i]);
                    else
                        xe = xe.Element(elements[i]);

                    if (i == elements.Length - 1)
                    {
                        if (string.IsNullOrEmpty(Attribute))
                            return xe.Value;
                        else
                            return xe.Attribute(Attribute).Value;
                    }
                }
            }
            catch { }

            return null;
        }

        public string GetDecrypt(string ElementPath, string Attribute, string XMLPath)
        {
            return CryptographyUtility.Decrypt(Get(ElementPath, Attribute, XMLPath));
        }

        public int GetInt(string ElementPath, string Attribute, string XMLPath)
        {
            return Cast.ToInt(Get(ElementPath, Attribute, XMLPath));
        }

        public HandleState Set(string ElementPath, string Attribute, string value, string XMLPath)
        {
            try
            {
                if (xml == null)
                    OpenConfig(XMLPath);

                string[] elements = ElementPath.Split('.');
                XElement xe = null;

                for (int i = 0; i < elements.Length; i++)
                {
                    if (xe == null)
                        xe = xml.Element(elements[i]);
                    else
                        xe = xe.Element(elements[i]);

                    if (i == elements.Length - 1)
                    {
                        if (string.IsNullOrEmpty(Attribute))
                            xe.Value = value;
                        else
                            xe.Attribute(Attribute).Value = value;
                    }
                }
            }
            catch (Exception ex) { return new HandleState(ex); }

            return new HandleState();
        }

        public int GetCountNode(string NodeName, string XMLPath)
        {
            try
            {
                OpenConfig(XMLPath);

                var result = xml.Descendants().Where(t => t.Name == NodeName)
                    .Select(x => new
                    {
                        NodeName = x.Name,
                        //NodeLevel = x.AncestorsAndSelf().Count(),
                        ChildsCount = x.Descendants().Count()
                    }).Distinct().FirstOrDefault();

                if (result != null)
                    return result.ChildsCount;
                return 0;
            }
            catch
            {
                return -1;
            }
        }
    }
}
