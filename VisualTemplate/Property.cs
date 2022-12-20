﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualTemplate
{
   public class Property: ICloneable, IComparable
    {
        private string _id;
        public string Id
        { get
            {
                return _id;
            }
            set
            {
                _id = value;
                Name = getNameById(_id);
                Description = getDescrById(_id);
            }
        }
        private string _type;
        public string Type
        {
            get
            {
                return _type;
            } set
            {
                _type = value;
                imgCode = Service.getImgCodeByType(_type);
            }
        }
        public string Value { get; set; }

        [JsonIgnore]
        public string Name { get; set; }
        [JsonIgnore]
        public string Description { get; set; }
        [JsonIgnore]
        public int imgCode { get; set; }
        public string Segment { get; set; }


        public Property(string id, string type, string value, string segment = "0")
        {
            Id = id;
            Type = type;
            Value = value;
            Segment = segment;
        }

        

        private int getImgCodeByType(string type)
        {
            switch (type)
            {
                case "Int1":
                    return 1;
                case "UInt1":
                    return 2;
                case "Int2":
                    return 3;
                case "UInt2":
                    return 4;
                case "Int4":
                    return 5;
                case "UInt4":
                    return 6;
                case "Int8":
                    return 7;
                case "UInt8":
                    return 8;
                case "Float":
                    return 9;
                case "Double":
                    return 10;
                case "Bool":
                    return 11;
                case "String":
                    return 12;
                case "Folder":
                    return 0;
                default:
                    return 13;
            }
        }

        private string getNameById(string id)
        {
            switch (id)
            {
                case "1":
                    return "CDT";
                case "2":
                    return "Value";
                case "3":
                    return "Quality";
                case "4":
                    return "Timestamp";
                case "5":
                    return "AccRight";
                case "6":
                    return "ScanRate";
                case "100":
                    return "EUnit";
                case "101":
                    return "Description";
                case "6500":
                    return "CopyVqt";
                case "5000":
                    return "Address";
                case "5001":
                    return "Active";
                case "5002":
                    return "RawValue";
                case "5100":
                    return "RecalcRawLow";
                case "5101":
                    return "RecalcRawMiddle";
                case "5102":
                    return "RecalcRawHigh";
                case "5103":
                    return "RecalcValLow";
                case "5104":
                    return "RecalcValMiddle";
                case "5105":
                    return "RecalcValHigh";
                case "5106":
                    return "RecalcTruncate";
                case "5107":
                    return "RecalcSetFailureQuality";
                case "5108":
                    return "RecalcInvert";
                case "6001":
                    return " ";
                case "6002":
                    return " ";
                case "6003":
                    return " ";
                case "6004":
                    return " ";
                case "6005":
                    return " ";
                case "6100":
                    return " ";
                case "7000":
                    return " ";
                case "8000":
                    return " ";
                case "9001":
                    return "Historizing";
                case "9002":
                    return "HistoryParams";
                case "10000":
                    return "EnableWriteVqt";
                case "777005":
                    return " ";
                case "777006":
                    return " ";
                case "777010":
                    return " ";
                case "777011":
                    return " ";
                case "777012":
                    return " ";
                case "777013":
                    return " ";
                case "777015":
                    return " ";
                case "777016":
                    return " ";
                case "777017":
                    return " ";
                case "777018":
                    return " ";
                case "6000":
                    return "NotAckEventCount";
                case "999000":
                    return "ObjectType";
                case "999001":
                    return "ObjectCode";
                case "999002":
                    return "ObjectSound";
                case "999003":
                    return "EventsEnabled";
                case "999004":
                    return "Conditions";
                case "999005":
                    return "IsAbstract";
                default:
                    return "";
            }
        }

        private string getDescrById(string id)
        {
            switch (id)
            {
                case "1":
                    return "CDT (Канонический тип данных)";
                case "2":
                    return "Значение";
                case "3":
                    return "Качество";
                case "4":
                    return "Метка времени";
                case "5":
                    return "Права доступа";
                case "6":
                    return "Скорость обновления";
                case "100":
                    return "Единицы измерения";
                case "101":
                    return "Описание сигнала";
                case "6500":
                    return "Записывать в сигнал перекладываемое значение";
                case "5000":
                    return "Адрес сигнала";
                case "5001":
                    return "Активный протокол";
                case "5002":
                    return "Физическое значение";
                case "5100":
                    return "Нижняя граница физического значения";
                case "5101":
                    return "Граница излома физического значения";
                case "5102":
                    return "Верхняя граница физического значения";
                case "5103":
                    return "Нижняя граница инженерного значения";
                case "5104":
                    return "Граница излома инженерного значения";
                case "5105":
                    return "Верхняя граница инженерного значения";
                case "5106":
                    return "Усекать значение по границе пересчета и добавлять в качество флаги усечения";
                case "5107":
                    return "При усечении по границе пересчета выставлять";
                case "5108":
                    return "Инвертировать логическое значение";
                case "6001":
                    return "Полное имя объекта, к которому ведёт данная ссылка";
                case "6002":
                    return "Разновидность ссылки";
                case "6003":
                    return "Автораскрытие ссылки";
                case "6004":
                    return "Константность ссылки";
                case "6005":
                    return "Свойство модуля OPC AE Server";
                case "6100":
                    return "Восприимчивость сигнала к изменениям";
                case "7000":
                    return "Ведение детального журнала изменений сигналов";
                case "8000":
                    return "Опциональная синхронизация при резервировании";
                case "9001":
                    return "Ведение истории";
                case "9002":
                    return "Дополнительные параметры сохранения истории";
                case "10000":
                    return "Постановка на обслуживание сигнала модулю Write VQT";
                case "777005":
                    return "Содержит определения внешних функций";
                case "777006":
                    return "Содержит карту дескрипторов сигнатур внешних функций";
                case "777010":
                    return "Формула для вычисления значения сигнала";
                case "777011":
                    return "Условие активации процедуры, определенной в свойстве 777012";
                case "777012":
                    return "Код процедуры на языке Alpha.Om , которая активируется по условию свойства 777011";
                case "777013":
                    return "Обработчик, срабатывающий перед отправкой события";
                case "777015":
                    return "Значение таймера (в миллисекундах) для исполнения процедуры";
                case "777016":
                    return " Код процедуры на языке Alpha.Om";
                case "777017":
                    return " Содержит cron";
                case "777018":
                    return "Cодержит код процедуры, которая будет выполняться по расписанию";
                case "6000":
                    return "Количество неквитированных событий";
                case "999000":
                    return "Тип объекта";
                case "999001":
                    return "Код объекта";
                case "999002":
                    return "Звук объекта";
                case "999003":
                    return "Признак генерации событий";
                case "999004":
                    return "Условия генерации событий";
                case "999005":
                    return "Тип абстрактный";
                default:
                    return "";
            }
        }

        public object Clone()
        {
            return new Property(Id, Type, Value);
        }

        public int CompareTo(object obj)
        {
            Property p = obj as Property;
            return Id.CompareTo(p.Id);
        }
    }

    public class PropertyComparer : IComparer<Property>
    {
        public int Compare(Property p1, Property p2)
        {
            if (int.Parse(p1.Id) > int.Parse(p2.Id))
                return 1;
            else if (int.Parse(p1.Id) < int.Parse(p2.Id))
                return -1;
            else
                return 0;            
        }
    }
}
