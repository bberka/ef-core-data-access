# EntityFrameworkCore.DataAccess

It's a modern and generic data access structure for .NET and Microsoft.EntityFrameworkCore. It supports UnitOfWork, Repository and QueryBuilder patterns. It also includes auto history utilities, multiple databases support with distributed transactions and databases/tables sharding for some database providers.

- [Implementation for EntityFrameworkCore 2.x](https://github.com/ffernandolima/ef-core-data-access/tree/ef-core-2)
- [Implementation for EntityFrameworkCore 3.x](https://github.com/ffernandolima/ef-core-data-access/tree/ef-core-3)
- [Implementation for EntityFrameworkCore 5.x](https://github.com/ffernandolima/ef-core-data-access/tree/ef-core-5)
- [Implementation for EntityFrameworkCore 6.x](https://github.com/ffernandolima/ef-core-data-access/tree/ef-core-6)
- [Implementation for EntityFrameworkCore 7.x](https://github.com/ffernandolima/ef-core-data-access/tree/ef-core-7)

## Give a Star! :star:

If you like or are using this project to learn or start your solution, please give it a star. Thanks!

## Why forked ?
The UnitOfWork inside the library gives DbContext access outside of UnitofWork also among other things that should not be available outside UnitOfWork class. This violates bunch of design principles.

I only been editing ef-core-6 branch not others and removing some properties/functions.

DbContext class itself already some what does same job what UnitOfWork does. It represents a single unit of a work. However UnitOfWork interface and design pattern limits the access to the DbContext

What i use in my projects like this;
Web API > Business > Persistance > DbContext

Persistance has UnitOfWork defined and Business has Services or Managers defined inside it. I never use DbContext outside of Persistance. Business services and managers only accesses to repositories through UnitOfWork and creates actual methods that can be used by Web or API project. 

The owner of the original repository, i dont know what he/she thinking but set a DbContext property in UnitOfWork as a public.

## What i did to improve ?
Merged Abstractions to single projects and implementation to single project
Re-named it
Removed some worrysome methods/properties from UnitOfWork (e.g. DbContext, transactions)
Made UnitOfWork a base abstract class so you have to actually create your own unit of work class and implement the necessary repositories inside it.
Since repositories provides almost same access as DbSet i'm not using them with an interface and initializing those in UnitOfWork constructor.
You should be creating an interface named IUnitOfWork define your repositories inside it. 
