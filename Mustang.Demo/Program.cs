using Mustang.BuilderSql;
using Mustang.Demo.Entity;
using Mustang.SqlBuilder;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Linq;

var entity = new Account() { Username = "杨川", Password = "123456", InDate = DateTime.Now };
//var entity2 = new RoleAccount() { Username = "杨川", Password = "123456", InDate = DateTime.Now };
var builderSql1 = new MySqlBuilder<Account>().Insert(entity).Builder();

var builderSql2 = new MySqlBuilder<Account>().Delete(entity).WhereCondition(ConditionRelation.NULL, account => account.Id, ConditionOperator.EqualTo, 1)
    .WhereCondition(ConditionRelation.AND, account => account.Password, ConditionOperator.EqualTo, "123s").Builder();

var builderSql3 = new MySqlBuilder<Account>().Update(entity).WhereCondition(ConditionRelation.NULL, account => account.Id, ConditionOperator.EqualTo, 1)
    .WhereCondition(ConditionRelation.AND, account => account.Password, ConditionOperator.EqualTo, "123s").Builder();

var builderSql4 = new MySqlBuilder<Account>()
    .Query()
    .WhereCondition(ConditionRelation.NULL, account => account.Id, ConditionOperator.EqualTo, 1)
    .WhereCondition(ConditionRelation.AND, account => account.Password, ConditionOperator.EqualTo, "123s")
    .OrderBy(w => w.Id, OrderByEnums.ASC)
    .Builder();

var builderSql5 = new MySqlBuilder<Account>()
.Query(new List<string>() { "Username", "Password" })
.WhereCondition(ConditionRelation.NULL, account => account.Id, ConditionOperator.EqualTo, 1)
.OrderBy(w => w.Id, OrderByEnums.ASC)
.Builder();

Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffffff"));
var builderSql6 = new MySqlBuilder<Account>()
.Query(new List<string>() { "Username", "Password" })
.InnerJoin<Account, RoleAccount>(account => account.Id, ConditionOperator.EqualTo, roleAccount => roleAccount.AccountId)
.JoinCondition<Account, RoleAccount>(account => account.Password, ConditionOperator.EqualTo, roleAccount => roleAccount.RoleName)
.WhereCondition(ConditionRelation.NULL, account => account.Id, ConditionOperator.EqualTo, 1)
.OrderBy(w => w.Id, OrderByEnums.ASC)
.Builder();
Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffffff"));
Console.WriteLine(builderSql6.Sql);
Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffffff"));

Console.WriteLine("MySQL");
//Console.WriteLine(builderSql1.Sql);
//Console.WriteLine(builderSql2.Sql);
//Console.WriteLine(builderSql3.Sql);
//Console.WriteLine(builderSql4.Sql);
//Console.WriteLine(builderSql5.Sql);


//var success = MustangDataAccess.ExecuteNonQuery(builderSql1);


StackTrace st = new StackTrace(skipFrames: 1, fNeedFileInfo: true);
StackFrame[] sfArray = st.GetFrames();

var ss= string.Join(" -> ",
     sfArray.Select(r =>
         $"{r.GetMethod().Name} in {r.GetFileName()} line:{r.GetFileLineNumber()} column:{r.GetFileColumnNumber()}"));
    
Console.WriteLine(ss);
Console.ReadLine();
