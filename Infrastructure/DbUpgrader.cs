using System;
using System.Reflection;
using DbUp;
using DbUp.Engine;

namespace Altidude.Infrastructure
{
  public class DbUpgrader
  {
    public static DatabaseUpgradeResult Upgrade(string connectionString, bool throwException)
    {
      var upgrader =
          DeployChanges.To
              .SqlDatabase(connectionString)
              .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
              .LogToConsole()
              .Build();

      var result = upgrader.PerformUpgrade();

      if (throwException && !result.Successful)
         throw result.Error;

      return result;
    }
  }
}