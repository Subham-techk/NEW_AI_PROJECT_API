using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure_Layer.Common.Database
{
    public interface IDbConnectionFactory
    {
        SqlConnection CreateConnection();
    }
}
