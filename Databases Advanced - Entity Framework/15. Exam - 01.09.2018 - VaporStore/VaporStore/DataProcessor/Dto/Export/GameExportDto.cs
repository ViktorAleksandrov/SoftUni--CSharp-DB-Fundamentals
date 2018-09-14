﻿namespace VaporStore.DataProcessor.Dto.Export
{
    using System.Xml.Serialization;

    [XmlType("Game")]
    public class GameExportDto
    {
        [XmlAttribute("title")]
        public string Title { get; set; }

        public string Genre { get; set; }

        public decimal Price { get; set; }
    }
}
