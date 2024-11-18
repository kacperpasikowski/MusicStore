using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project.Models;

namespace Project.Specifications
{
	public class ProductWithDetailSpecification : BaseSpecification<Product>
	{
		public ProductWithDetailSpecification(int id) 
		:base (x => x.Id ==id)
		{
			AddInclude(x=> x.Brand);
			AddInclude (x => x.ProductType);
		}
	}
}