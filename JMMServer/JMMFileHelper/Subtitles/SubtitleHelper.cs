﻿using System;
using System.Collections.Generic;
using System.Linq;
using JMMContracts.PlexContracts;


namespace JMMFileHelper.Subtitles
{
    public class SubtitleHelper
    {
        private static Dictionary<string, string> _iso639_1_iso639_3;
        private static Dictionary<string, string> _iso639_1_lan;
        private static Dictionary<string, string> _lan_lower;


        public static List<Stream> GetSubtitleStreams(string fullpath)
        {
            List<Stream> ls = new VobSubSubtitles().Process(fullpath);
            ls.AddRange(new TextSubtitles().Process(fullpath));
            return ls;
        }

        public static Dictionary<string, string> Iso639_1_TO_Iso639_3
        {
            get
            {
                if (_iso639_1_iso639_3 == null)
                {
                    _iso639_1_iso639_3 = new Dictionary<string, string>();
                    foreach (string k in Iso639_3_TO_Iso639_1.Keys)
                    {
                        string val = Iso639_3_TO_Iso639_1[k];
                        if (!_iso639_1_iso639_3.ContainsKey(val))
                            _iso639_1_iso639_3.Add(val, k);
                    }
                }
                return _iso639_1_iso639_3;
            }
        }
        public static Dictionary<string, string> Iso639_1_TO_Languages
        {
            get
            {
                return _iso639_1_lan ?? (_iso639_1_lan = Languages_TO_ISO639_1.ToDictionary(a => a.Value, a => a.Key));
            }
        }
        public static Dictionary<string, string> Languages_TO_ISO639_1_Lower
        {
            get
            {
                return _lan_lower ?? (_lan_lower = Languages_TO_ISO639_1.ToDictionary(a => a.Key.ToLower(), a => a.Value));
            }
        }
        public static Dictionary<string, string> Iso639_3_TO_Iso639_1 = new Dictionary<string, string>
                                                     {
                                                         {"aar", "aa"},
                                                         {"abk", "ab"},
                                                         {"afr", "af"},
                                                         {"aka", "ak"},
                                                         {"amh", "am"},
                                                         {"ara", "ar"},
                                                         {"arg", "an"},
                                                         {"asm", "as"},
                                                         {"ava", "av"},
                                                         {"ave", "ae"},
                                                         {"aym", "ay"},
                                                         {"aze", "az"},
                                                         {"bak", "ba"},
                                                         {"bam", "bm"},
                                                         {"bel", "be"},
                                                         {"ben", "bn"},
                                                         {"bih", "bh"},
                                                         {"bis", "bi"},
                                                         {"bos", "bs"},
                                                         {"bre", "br"},
                                                         {"bul", "bg"},
                                                         {"bur", "my"},
                                                         {"cat", "ca"},
                                                         {"cha", "ch"},
                                                         {"che", "ce"},
                                                         {"chi", "zh"},
                                                         {"chu", "cu"},
                                                         {"chv", "cv"},
                                                         {"cor", "kw"},
                                                         {"cos", "co"},
                                                         {"cre", "cr"},
                                                         {"cze", "cs"},
                                                         {"dan", "da"},
                                                         {"div", "dv"},
                                                         {"dzo", "dz"},
                                                         {"ell", "el"},
                                                         {"eng", "en"},
                                                         {"epo", "eo"},
                                                         {"est", "et"},
                                                         {"ewe", "ee"},
                                                         {"baq", "eu"},
                                                         {"fao", "fo"},
                                                         {"fij", "fj"},
                                                         {"fin", "fi"},
                                                         {"fre", "fr"},
                                                         {"fry", "fy"},
                                                         {"ful", "ff"},
                                                         {"geo", "ka"},
                                                         {"ger", "de"},
                                                         {"gla", "gd"},
                                                         {"gle", "ga"},
                                                         {"glg", "gl"},
                                                         {"grn", "gn"},
                                                         {"guj", "gu"},
                                                         {"glv", "gv"},
                                                         {"hat", "ht"},
                                                         {"hau", "ha"},
                                                         {"heb", "he"},
                                                         {"her", "hz"},
                                                         {"hin", "hi"},
                                                         {"hmo", "ho"},
                                                         {"hrv", "hr"},
                                                         {"hun", "hu"},
                                                        {"arm", "hy"},

                                                         {"ibo", "ig"},
                                                         {"ice", "is"},
                                                         {"ido", "io"},
                                                         {"iii", "ii"},
                                                         {"iku", "iu"},
                                                         {"ile", "ie"},
                                                         {"ina", "ia"},
                                                         {"ind", "id"},
                                                         {"ipk", "ik"},
                                                         {"ita", "it"},
                                                         {"jav", "jv"},
                                                         {"jpn", "ja"},
                                                         {"kal", "kl"},
                                                         {"kan", "kn"},
                                                         {"kas", "ks"},
                                                         {"kau", "kr"},
                                                         {"kaz", "kk"},
                                                         {"khm", "km"},
                                                         {"kik", "ki"},
                                                         {"kin", "rw"},
                                                         {"kir", "ky"},
                                                         {"kom", "kv"},
                                                         {"kon", "kg"},
                                                         {"kor", "ko"},
                                                         {"kua", "kj"},
                                                         {"kur", "ku"},
                                                         {"lao", "lo"},
                                                         {"lat", "la"},
                                                         {"lav", "lv"},
                                                         {"lim", "li"},
                                                         {"lin", "ln"},
                                                         {"lit", "lt"},
                                                         {"ltz", "lb"},
                                                         {"lub", "lu"},
                                                         {"lug", "lg"},
                                                         {"mac", "mk"},
                                                         {"mah", "mh"},
                                                         {"mal", "ml"},
                                                         {"mao", "mi"},
                                                         {"mar", "mr"},
                                                         {"may", "ms"},
                                                         {"mlg", "mg"},
                                                         {"mlt", "mt"},
                                                         {"mol", "mo"},
                                                         {"mon", "mn"},
                                                         {"nau", "na"},
                                                         {"nav", "nv"},
                                                         {"nbl", "nr"},
                                                         {"nde", "nd"},
                                                         {"ndo", "ng"},
                                                        {"dut", "nl"},

                                                         {"nep", "ne"},
                                                         {"nno", "nn"},
                                                         {"nob", "nb"},
                                                         {"nor", "no"},
                                                         {"nya", "ny"},
                                                         {"oci", "oc"},
                                                         {"oji", "oj"},
                                                         {"ori", "or"},
                                                         {"orm", "om"},
                                                         {"oss", "os"},
                                                         {"pan", "pa"},
                                                         {"per", "fa"},
                                                         {"pli", "pi"},
                                                         {"pol", "pl"},
                                                         {"por", "pt"},
                                                         {"pus", "ps"},
                                                         {"que", "qu"},
                                                         {"roh", "rm"},
                                                         {"run", "rn"},
                                                         {"rus", "ru"},
                                                         {"sag", "sg"},
                                                         {"san", "sa"},
                                                         {"scc", "sr"},
                                                         {"srp", "sr"},
                                                         {"sin", "si"},
                                                         {"slo", "sk"},
                                                         {"slv", "sl"},
                                                         {"sme", "se"},
                                                         {"smo", "sm"},
                                                         {"sna", "sn"},
                                                         {"snd", "sd"},
                                                         {"som", "so"},
                                                        {"alb", "sq"},

                                                         {"sot", "st"},
                                                         {"spa", "es"},
                                                         {"srd", "sc"},
                                                         {"ssw", "ss"},
                                                         {"sun", "su"},
                                                         {"swa", "sw"},
                                                         {"swe", "sv"},
                                                         {"tah", "ty"},
                                                         {"tam", "ta"},
                                                         {"tat", "tt"},
                                                         {"tel", "te"},
                                                         {"tgk", "tg"},
                                                         {"tgl", "tl"},
                                                         {"tha", "th"},
                                                         {"tib", "bo"},
                                                         {"tir", "ti"},
                                                         {"ton", "to"},
                                                         {"tsn", "tn"},
                                                         {"tso", "ts"},
                                                         {"tuk", "tk"},
                                                         {"tur", "tr"},
                                                         {"twi", "tw"},
                                                         {"uig", "ug"},
                                                         {"ukr", "uk"},
                                                         {"urd", "ur"},
                                                         {"uzb", "uz"},
                                                         {"ven", "ve"},
                                                         {"vie", "vi"},
                                                         {"vol", "vo"},
                                                         {"wel", "cy"},
                                                         {"wln", "wa"},
                                                         {"wol", "wo"},
                                                         {"xho", "xh"},
                                                         {"yid", "yi"},
                                                         {"yor", "yo"},
                                                         {"zha", "za"},
                                                         {"zul", "zu"},
                                                         {"rum", "ro"},
                                                         {"ron", "ro"},
                                                         {"pob", "pb"},
                                                         {"unk", "xx"},
                                                     };

        public static Dictionary<string, string> Extensions = new Dictionary<string, string>()
                                                              {
                                                                  {"utf", "text/plain"},
                                                                  {"utf8", "text/plain"},
                                                                  {"utf-8", "text/plain"},
                                                                  {"srt", "text/plain"},
                                                                  {"smi", "text/plain"},
                                                                  {"rt", "text/plain"},
                                                                  {"ssa", "text/plain"},
                                                                  {"aqt", "text/plain"},
                                                                  {"jss", "text/plain"},
                                                                  {"ass", "text/plain"},
                                                                  {"idx", "application/octet-stream"},
                                                                  {"sub", "application/octet-stream"},
                                                                  {"txt", "text/plain"},
                                                                  {"psb", "text/plain"}
                                                              };
        public static Dictionary<string, string> Languages_TO_ISO639_1 = new Dictionary<string, string>
                                                      {
                                                          {"Unknown", "xx"},
                                                          {"Afar", "aa"},
                                                          {"Abkhazian", "ab"},
                                                          {"Afrikaans", "af"},
                                                          {"Akan", "ak"},
                                                          {"Albanian", "sq"},
                                                          {"Amharic", "am"},
                                                          {"Arabic", "ar"},
                                                          {"Aragonese", "an"},
                                                          {"Armenian", "hy"},
                                                          {"Assamese", "as"},
                                                          {"Avaric", "av"},
                                                          {"Avestan", "ae"},
                                                          {"Aymara", "ay"},
                                                          {"Azerbaijani", "az"},
                                                          {"Bashkir", "ba"},
                                                          {"Bambara", "bm"},
                                                          {"Basque", "eu"},
                                                          {"Belarusian", "be"},
                                                          {"Bengali", "bn"},
                                                          {"Bihari", "bh"},
                                                          {"Bislama", "bi"},
                                                          {"Bosnian", "bs"},
                                                          {"Breton", "br"},
                                                          {"Bulgarian", "bg"},
                                                          {"Burmese", "my"},
                                                          {"Catalan", "ca"},
                                                          {"Chamorro", "ch"},
                                                          {"Chechen", "ce"},
                                                          {"Chinese", "zh"},
                                                          {"ChurchSlavic", "cu"},
                                                          {"Chuvash", "cv"},
                                                          {"Cornish", "kw"},
                                                          {"Corsican", "co"},
                                                          {"Cree", "cr"},
                                                          {"Czech", "cs"},
                                                          {"Danish", "da"},
                                                          {"Divehi", "dv"},
                                                          {"Dutch", "nl"},
                                                          {"Dzongkha", "dz"},
                                                          {"English", "en"},
                                                          {"Esperanto", "eo"},
                                                          {"Estonian", "et"},
                                                          {"Ewe", "ee"},
                                                          {"Faroese", "fo"},
                                                          {"Fijian", "fj"},
                                                          {"Finnish", "fi"},
                                                          {"French", "fr"},
                                                          {"Frisian", "fy"},
                                                          {"Fulah", "ff"},
                                                          {"Georgian", "ka"},
                                                          {"German", "de"},
                                                          {"Gaelic", "gd"},
                                                          {"Irish", "ga"},
                                                          {"Galician", "gl"},
                                                          {"Manx", "gv"},
                                                          {"Greek", "el"},
                                                          {"Guarani", "gn"},
                                                          {"Gujarati", "gu"},
                                                          {"Haitian", "ht"},
                                                          {"Hausa", "ha"},
                                                          {"Hebrew", "he"},
                                                          {"Herero", "hz"},
                                                          {"Hindi", "hi"},
                                                          {"HiriMotu", "ho"},
                                                          {"Croatian", "hr"},
                                                          {"Hungarian", "hu"},
                                                          {"Igbo", "ig"},
                                                          {"Icelandic", "is"},
                                                          {"Ido", "io"},
                                                          {"SichuanYi", "ii"},
                                                          {"Inuktitut", "iu"},
                                                          {"Interlingue", "ie"},
                                                          {"Interlingua", "ia"},
                                                          {"Indonesian", "id"},
                                                          {"Inupiaq", "ik"},
                                                          {"Italian", "it"},
                                                          {"Javanese", "jv"},
                                                          {"Japanese", "ja"},
                                                          {"Kalaallisut", "kl"},
                                                          {"Kannada", "kn"},
                                                          {"Kashmiri", "ks"},
                                                          {"Kanuri", "kr"},
                                                          {"Kazakh", "kk"},
                                                          {"Khmer", "km"},
                                                          {"Kikuyu", "ki"},
                                                          {"Kinyarwanda", "rw"},
                                                          {"Kirghiz", "ky"},
                                                          {"Komi", "kv"},
                                                          {"Kongo", "kg"},
                                                          {"Korean", "ko"},
                                                          {"Kuanyama", "kj"},
                                                          {"Kurdish", "ku"},
                                                          {"Lao", "lo"},
                                                          {"Latin", "la"},
                                                          {"Latvian", "lv"},
                                                          {"Limburgan", "li"},
                                                          {"Lingala", "ln"},
                                                          {"Lithuanian", "lt"},
                                                          {"Luxembourgish", "lb"},
                                                          {"LubaKatanga", "lu"},
                                                          {"Ganda", "lg"},
                                                          {"Macedonian", "mk"},
                                                          {"Marshallese", "mh"},
                                                          {"Malayalam", "ml"},
                                                          {"Maori", "mi"},
                                                          {"Marathi", "mr"},
                                                          {"Malay", "ms"},
                                                          {"Malagasy", "mg"},
                                                          {"Maltese", "mt"},
                                                          {"Moldavian", "mo"},
                                                          {"Mongolian", "mn"},
                                                          {"Nauru", "na"},
                                                          {"Navajo", "nv"},
                                                          {"SouthNdebele", "nr"},
                                                          {"NorthNdebele", "nd"},
                                                          {"Ndonga", "ng"},
                                                          {"Nepali", "ne"},
                                                          {"NorwegianNynorsk", "nn"},
                                                          {"NorwegianBokmal", "nb"},
                                                          {"Norwegian", "no"},
                                                          {"Chichewa", "ny"},
                                                          {"Occitan", "oc"},
                                                          {"Ojibwa", "oj"},
                                                          {"Oriya", "or"},
                                                          {"Oromo", "om"},
                                                          {"Ossetian", "os"},
                                                          {"Panjabi", "pa"},
                                                          {"Persian", "fa"},
                                                          {"Pali", "pi"},
                                                          {"Polish", "pl"},
                                                          {"Portuguese", "pt"},
                                                          {"Pushto", "ps"},
                                                          {"Quechua", "qu"},
                                                          {"RaetoRomance", "rm"},
                                                          {"Romanian", "ro"},
                                                          {"Rundi", "rn"},
                                                          {"Russian", "ru"},
                                                          {"Sango", "sg"},
                                                          {"Sanskrit", "sa"},
                                                          {"Serbian", "sr"},
                                                          {"Sinhalese", "si"},
                                                          {"Slovak", "sk"},
                                                          {"Slovenian", "sl"},
                                                          {"Sami", "se"},
                                                          {"Samoan", "sm"},
                                                          {"Shona", "sn"},
                                                          {"Sindhi", "sd"},
                                                          {"Somali", "so"},
                                                          {"Sotho", "st"},
                                                          {"Spanish", "es"},
                                                          {"Sardinian", "sc"},
                                                          {"Swati", "ss"},
                                                          {"Sundanese", "su"},
                                                          {"Swahili", "sw"},
                                                          {"Swedish", "sv"},
                                                          {"Tahitian", "ty"},
                                                          {"Tamil", "ta"},
                                                          {"Tatar", "tt"},
                                                          {"Telugu", "te"},
                                                          {"Tajik", "tg"},
                                                          {"Tagalog", "tl"},
                                                          {"Thai", "th"},
                                                          {"Tibetan", "bo"},
                                                          {"Tigrinya", "ti"},
                                                          {"Tonga", "to"},
                                                          {"Tswana", "tn"},
                                                          {"Tsonga", "ts"},
                                                          {"Turkmen", "tk"},
                                                          {"Turkish", "tr"},
                                                          {"Twi", "tw"},
                                                          {"Uighur", "ug"},
                                                          {"Ukrainian", "uk"},
                                                          {"Urdu", "ur"},
                                                          {"Uzbek", "uz"},
                                                          {"Venda", "ve"},
                                                          {"Vietnamese", "vi"},
                                                          {"Volapuk", "vo"},
                                                          {"Welsh", "cy"},
                                                          {"Walloon", "wa"},
                                                          {"Wolof", "wo"},
                                                          {"Xhosa", "xh"},
                                                          {"Yiddish", "yi"},
                                                          {"Yoruba", "yo"},
                                                          {"Zhuang", "za"},
                                                          {"Zulu", "zu"},
                                                          {"Brazilian", "pb"},
                                                          {"NoLanguage", "xn"}
                                                      };
    }
}
