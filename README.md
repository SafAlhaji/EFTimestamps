# EFTimestamps
A library for handling timestamps &amp; soft deletes in Entity Framework.

## Documentation
* [Introduction](#introduction)
* [Getting started](#getting-started)
* [Usage](#usage)
* [Methods](#methods)
* [Notes](#notes)

<a name="introduction"></a>
## Introduction
EFTimestamps is a library that control the dates your models have been created, updated, or (soft) deleted, automatically handling all CRUD operations in the DB context.
When models are soft deleted, they are not actually deleted from the database, instead a ```DeletedAt``` column is set to the date the model has been deleted. If a model's ```DeletedAt``` value is not null, and is set to a specific date, it means it has been deleted.


<a name="getting-started"></a>
## Getting started
EFTimestamps library is available in nuget.org. To install it, run the following command in the Package Manager Console:
```Install-Package EFTimestamps```

Next, let your DB context inheirt from ```TimestampsDbContext```, as follows:
```csharp
public class ApplicationDbContext : TimestampsDbContext
{
}
```

<a name="usage"></a>
## Usage
For a model to have the columns ```CreatedAt``` and ```UpdatedAt```, let it implement the ```ITimestamps``` interface and all its members, as follows:
```csharp
public class Person : ITimestamps
{
  public int Id { get; set; }
  public string Name { get; set; }
  
  // ITimestamps implementation
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
}
```

Along with timestamps, a model can be soft-deletable by implementing the ```ISoftDeletable``` interface as follows:
```csharp
public class Person : ITimestamps, ISoftDeletable
{
  public int Id { get; set; }
  public string Name { get; set; }
  
  // ITimestamps implementation
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
  
  // ISoftDeletable implementation
  public DateTime? DeletedAt { get; set; }
}
```

That's it! Your model can now have timestamps via the ```CreatedAt``` and ```UpdatedAt``` columns, as well as be soft deleted via the ```DeletedAt``` columns.

<a name="methods"></a>
## Methods
You can use the following helper methods to extend your use of soft deletes:

* Soft deleting a model:
```csharp
var person = DbContext.Persons.First();
DbContext.Persons.Remove(person);
```

* Restoring a model:
```csharp
DbContext.Persons.Restore(person);
```

* Checking if a model is deleted:
```csharp
person.IsDeleted();
```

* If you don't want a model to be soft-deleted, and want it permanently removed, you can force-remove it:
```csharp
DbContext.Persons.ForceRemove(person);
```

<a name="notes"></a>
## Notes
```TimestampsDbContext``` is currently incompatible with ```IdentityDbContext``` used in ASP.NET MVC projects. It will however be available hopefully in the next versions of the library.
