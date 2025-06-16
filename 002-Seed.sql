

Insert into ItemStatus (ItemStatusId,Name)
values (1, 'Private') ,(2, 'Shared') , (3,'Retired')

Insert into Party (PartyId,Name)
Select NEWID(), 'Party 1'

Insert into Party (PartyId,Name)
Select NEWID(), 'Party 2'

Insert into Party (PartyId,Name)
Select NEWID(), 'Party 3'

Insert into Party (PartyId,Name)
Select NEWID(), 'Party 4'

Declare @partyId uniqueidentifier;
Declare @userId uniqueidentifier;
declare @itemId uniqueidentifier;

Select @partyId = partyId from Party where name = 'Party 1'
Select @userId = NEWID();
select @itemId = NEWID();

Insert Into [User] (UserId, FirstName , LastName, PartyId)
values (@userId, 'Andre','Cavalheiro Party 1',@partyId)

insert into Item (ItemId, ItemStatusId, Name,CreatedById , CreatedDate)
values (@itemId, 1,'Item 1', @userId, GETUTCDATE())

insert into ItemParty(ItemId,PartyId)
values (@itemId,@partyId)

Select @partyId = partyId from Party where name = 'Party 2'
Select @userId = NEWID();
select @itemId = NEWID();

Insert Into [User] (UserId, FirstName , LastName, PartyId)
values (@userId, 'FirstName 2','LastName 2 Party 2',@partyId)

insert into Item (ItemId, ItemStatusId, Name,CreatedById , CreatedDate)
values (@itemId, 2,'Item 2', @userId, GETUTCDATE())

insert into ItemParty(ItemId,PartyId)
values (@itemId,@partyId)

select @itemId = NEWID();

insert into Item (ItemId, ItemStatusId, Name,CreatedById , CreatedDate)
values (@itemId, 2,'Item 3', @userId, GETUTCDATE())

insert into ItemParty(ItemId,PartyId)
values (@itemId,@partyId)

Select @partyId = partyId from Party where name = 'Party 3'
Select @userId = NEWID();
select @itemId = NEWID();

Insert Into [User] (UserId, FirstName , LastName, PartyId)
values (@userId, 'FirstName 3','LastName 3 Party 3',@partyId)

insert into Item (ItemId, ItemStatusId, Name,CreatedById , CreatedDate)
values (@itemId, 2,'Item 4', @userId, GETUTCDATE())

insert into ItemParty(ItemId,PartyId)
values (@itemId,@partyId)

Select @partyId = partyId from Party where name = 'Party 4'
Select @userId = NEWID();
select @itemId = NEWID();

Insert Into [User] (UserId, FirstName , LastName, PartyId)
values (@userId, 'FirstName 4','LastName 4 Party 4',@partyId)

insert into Item (ItemId, ItemStatusId, Name,CreatedById , CreatedDate)
values (@itemId, 2,'Item 5', @userId, GETUTCDATE())

insert into ItemParty(ItemId,PartyId)
values (@itemId,@partyId)

insert into ProposalType (ProposalTypeId, Name)
values (1, 'Initial'),
(2, 'Counter')

insert into ProposalStatus (ProposalStatusId, Name)
values (1, 'New'),
 (2, 'Approved'),
 (3, 'Rejected'),
 (4, 'Abandoned')



insert into ProposalAllocationType (ProposalAllocationTypeId, Name)
values (1, 'Percentage'), 
(2,'Amount')






