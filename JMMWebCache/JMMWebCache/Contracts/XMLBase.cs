﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using System.Xml;
using System.Text;

namespace OMMWebCache.Contracts
{
	public class XMLBase
	{
		public string ToXML()
		{
			XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
			ns.Add("", "");

			XmlSerializer serializer = new XmlSerializer(this.GetType());
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.OmitXmlDeclaration = true; // Remove the <?xml version="1.0" encoding="utf-8"?>

			StringBuilder sb = new StringBuilder();
			XmlWriter writer = XmlWriter.Create(sb, settings);
			serializer.Serialize(writer, this, ns);

			return sb.ToString();
		}
	}
}