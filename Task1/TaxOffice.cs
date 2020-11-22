using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task1.CustomAttributes;

namespace Task1
{
    //Налогова служба
    [UInfo(Desc = "Описание класса сущности Налоговая служба")]
    public class TaxOffice
    {
        private readonly List<TaxPayer> TaxPayers = new List<TaxPayer>();
        private readonly List<Revenue> Revenues = new List<Revenue>();
        private readonly List<RevenueSource> RevenueSources = new List<RevenueSource>();

        public TaxOffice()
        {
            //initialization
            var taxPayer1 = new TaxPayer()
            {
                Id = Guid.NewGuid(),
                Address = "Address 1",
                FullName = "Tax Payer 1",
                Telephone = "+1234567890",
                OtherInfo = String.Empty
            };
            var taxPayer2 = new TaxPayer()
            {
                Id = Guid.NewGuid(),
                Address = "Address 2",
                FullName = "Tax Payer 2",
                Telephone = "+1234567891",
                OtherInfo = String.Empty
            };
            TaxPayers = new List<TaxPayer>()
            {
                taxPayer1,
                taxPayer2
            };
            var revenueSource1 = new RevenueSource()
            {
                Id = Guid.NewGuid(),
                Name = "Salary",
                OrganizationName = "Org1"
            };
            var revenueSource2 = new RevenueSource()
            {
                Id = Guid.NewGuid(),
                Name = "Salary",
                OrganizationName = "Org2"
            };
            RevenueSources = new List<RevenueSource>()
            {
                revenueSource1,
                revenueSource2
            };
            Revenues = new List<Revenue>()
            {
                new Revenue()
                {
                    Id = Guid.NewGuid(),
                    Amount = 155.5,
                    DateOfReceipt = DateTime.Now,
                    RevenueSource = revenueSource1,
                    RevenueSourceId = revenueSource1.Id,
                    TaxPayer = taxPayer1,
                    TaxPayerId = taxPayer1.Id
                },
                new Revenue()
                {
                    Id = Guid.NewGuid(),
                    Amount = 1255,
                    DateOfReceipt = DateTime.Now,
                    RevenueSource = revenueSource2,
                    RevenueSourceId = revenueSource2.Id,
                    TaxPayer = taxPayer2,
                    TaxPayerId = taxPayer2.Id
                },
            };
        }
        public List<string> CollectTaxes()
        {
            var response = new List<string>();
            foreach (var item in Revenues)
            {
                var message = String.Format("Tax was collected from {0} with amount {1}", item.TaxPayer.FullName, item.Amount * 0.2);
                Console.WriteLine(message);
                response.Add(message);
            }
            return response;
        }
        public List<TaxPayer> GetAllTaxPayers()
        {
            foreach (var item in TaxPayers)
            {
                Console.WriteLine(String.Format("Tax payer {0}", item.FullName));
            }
            return TaxPayers;
        }

        public List<Revenue> GetAllRevenues()
        {
            foreach (var item in Revenues)
            {
                Console.WriteLine(String.Format("Revenue {0}", item.Id));
            }
            return Revenues;
        }

        public void AddTaxPayer(TaxPayer taxPayer)
        {
            TaxPayers.Add(taxPayer);
        }

        public void UpdateTaxPayer(TaxPayer taxPayer)
        {
            var existedTaxPayer = TaxPayers.Where(payer =>
            {
                return payer.Id == taxPayer.Id;
            }).FirstOrDefault();
            if (existedTaxPayer == null)
            {
                throw new Exception(String.Format("Tax payer {0} does not exist", taxPayer.FullName));
            }
            TaxPayers.Remove(existedTaxPayer);
            TaxPayers.Add(taxPayer);
        }

        public void AddRevenue(Revenue revenue)
        {
            Revenues.Add(revenue);
        }
    }

    //Платник налогів
    public class TaxPayer
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public string OtherInfo { get; set; }
    }

    //Дохід
    public class Revenue
    {
        public Guid Id { get; set; }
        public DateTime DateOfReceipt { get; set; }
        public double Amount { get; set; }

        public Guid TaxPayerId { get; set; }
        public TaxPayer TaxPayer { get; set; }
        public Guid RevenueSourceId { get; set; }
        public RevenueSource RevenueSource { get; set; }
    }

    //Джерело доходу
    public class RevenueSource
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string OrganizationName { get; set; }
    }
}
