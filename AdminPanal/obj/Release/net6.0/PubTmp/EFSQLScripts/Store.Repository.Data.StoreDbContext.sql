IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230611223637_init')
BEGIN
    CREATE TABLE [DeliveryMethods] (
        [Id] int NOT NULL IDENTITY,
        [ShortName] nvarchar(max) NOT NULL,
        [Description] nvarchar(max) NOT NULL,
        [DeliveryTime] nvarchar(max) NOT NULL,
        [Cost] decimal(18,2) NOT NULL,
        CONSTRAINT [PK_DeliveryMethods] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230611223637_init')
BEGIN
    CREATE TABLE [ProductBrands] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_ProductBrands] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230611223637_init')
BEGIN
    CREATE TABLE [ProductTypes] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_ProductTypes] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230611223637_init')
BEGIN
    CREATE TABLE [Orders] (
        [Id] int NOT NULL IDENTITY,
        [BuyerEmail] nvarchar(max) NOT NULL,
        [OrderDate] datetimeoffset NOT NULL,
        [Status] nvarchar(max) NOT NULL,
        [ShippingAddress_FirstName] nvarchar(max) NOT NULL,
        [ShippingAddress_LastName] nvarchar(max) NOT NULL,
        [ShippingAddress_Street] nvarchar(max) NOT NULL,
        [ShippingAddress_City] nvarchar(max) NOT NULL,
        [ShippingAddress_Country] nvarchar(max) NOT NULL,
        [DeliveryMethodId] int NOT NULL,
        [SubTotal] decimal(18,2) NOT NULL,
        [PaymentIntentId] nvarchar(max) NULL,
        CONSTRAINT [PK_Orders] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Orders_DeliveryMethods_DeliveryMethodId] FOREIGN KEY ([DeliveryMethodId]) REFERENCES [DeliveryMethods] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230611223637_init')
BEGIN
    CREATE TABLE [Products] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        [Description] nvarchar(max) NOT NULL,
        [PictureUrl] nvarchar(max) NOT NULL,
        [Price] decimal(18,2) NOT NULL,
        [ProductBrandId] int NOT NULL,
        [ProductTypeId] int NOT NULL,
        CONSTRAINT [PK_Products] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Products_ProductBrands_ProductBrandId] FOREIGN KEY ([ProductBrandId]) REFERENCES [ProductBrands] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Products_ProductTypes_ProductTypeId] FOREIGN KEY ([ProductTypeId]) REFERENCES [ProductTypes] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230611223637_init')
BEGIN
    CREATE TABLE [OrderItems] (
        [Id] int NOT NULL IDENTITY,
        [Product_ProductId] int NOT NULL,
        [Product_ProductName] nvarchar(max) NOT NULL,
        [Product_PictureUrl] nvarchar(max) NOT NULL,
        [Price] decimal(18,2) NOT NULL,
        [Quantity] int NOT NULL,
        [OrderId] int NULL,
        CONSTRAINT [PK_OrderItems] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_OrderItems_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230611223637_init')
BEGIN
    CREATE TABLE [ProductRatings] (
        [Id] int NOT NULL IDENTITY,
        [ProductId] int NOT NULL,
        [UserName] nvarchar(max) NOT NULL,
        [Rating] int NOT NULL,
        [Message] nvarchar(max) NOT NULL,
        [Email] nvarchar(max) NOT NULL,
        [DateTime] datetime2 NOT NULL,
        CONSTRAINT [PK_ProductRatings] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ProductRatings_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230611223637_init')
BEGIN
    CREATE INDEX [IX_OrderItems_OrderId] ON [OrderItems] ([OrderId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230611223637_init')
BEGIN
    CREATE INDEX [IX_Orders_DeliveryMethodId] ON [Orders] ([DeliveryMethodId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230611223637_init')
BEGIN
    CREATE UNIQUE INDEX [IX_ProductBrands_Name] ON [ProductBrands] ([Name]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230611223637_init')
BEGIN
    CREATE INDEX [IX_ProductRatings_ProductId] ON [ProductRatings] ([ProductId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230611223637_init')
BEGIN
    CREATE INDEX [IX_Products_ProductBrandId] ON [Products] ([ProductBrandId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230611223637_init')
BEGIN
    CREATE INDEX [IX_Products_ProductTypeId] ON [Products] ([ProductTypeId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230611223637_init')
BEGIN
    CREATE UNIQUE INDEX [IX_ProductTypes_Name] ON [ProductTypes] ([Name]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230611223637_init')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230611223637_init', N'6.0.18');
END;
GO

COMMIT;
GO

