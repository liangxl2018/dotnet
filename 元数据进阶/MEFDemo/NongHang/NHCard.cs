using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using BankInterface;

namespace NongHang
{
   [ExportCardAttribute(CardType="NongHang")]
   public class NHCard : ICard
   {
      public string GetCountInfo()
      {
         return "Nong Ye Yin Hang";
      }

      public void SaveMoney(double money)
      {
         this.Money += money;
      }

      public void CheckOutMoney(double money)
      {
         this.Money -= money;
      }

      public double Money { get; set; }
   }
}
