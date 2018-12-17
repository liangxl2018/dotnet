using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BankInterface;
using System.ComponentModel.Composition;

namespace BankOfChina
{
   [ExportCardAttribute(CardType="BankOfChina")]
   public class ZHCard : ICard
   {
      public string GetCountInfo()
      {
         return "Bank Of China";
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
