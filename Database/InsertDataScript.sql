USE [BillingDb]
GO
SET IDENTITY_INSERT [dbo].[Customers] ON 
GO
INSERT [dbo].[Customers] ([Id], [Name], [Phone], [Address]) VALUES (1, N'SAKIB', N'+8801725452626', N'Magura, Khulna, Bangladesh')
GO
SET IDENTITY_INSERT [dbo].[Customers] OFF
GO
SET IDENTITY_INSERT [dbo].[Inventories] ON 
GO
INSERT [dbo].[Inventories] ([Id], [Date], [BillNo], [CustomerId], [TotalDiscount], [TotalBillAmount], [DueAmount], [PaidAmount]) VALUES (5, CAST(N'2023-07-09T00:00:00.000' AS DateTime), N'01-2023-07-09', 1, 935, 5088, 88, 5000)
GO
INSERT [dbo].[Inventories] ([Id], [Date], [BillNo], [CustomerId], [TotalDiscount], [TotalBillAmount], [DueAmount], [PaidAmount]) VALUES (8, CAST(N'2023-07-10T00:00:00.000' AS DateTime), N'02-2023-07-10', 1, 23, 4231.9, 2231.9, 2000)
GO
INSERT [dbo].[Inventories] ([Id], [Date], [BillNo], [CustomerId], [TotalDiscount], [TotalBillAmount], [DueAmount], [PaidAmount]) VALUES (10, CAST(N'2023-07-10T00:00:00.000' AS DateTime), N'03-2023-07-10', 1, 1002, 12958, 2958, 10000)
GO
SET IDENTITY_INSERT [dbo].[Inventories] OFF
GO
SET IDENTITY_INSERT [dbo].[Products] ON 
GO
INSERT [dbo].[Products] ([Id], [Code], [Name], [Rate]) VALUES (1, N'R-1', N'Rice', 56.3)
GO
INSERT [dbo].[Products] ([Id], [Code], [Name], [Rate]) VALUES (2, N'F-1', N'Fan 56 Inch', 2960)
GO
INSERT [dbo].[Products] ([Id], [Code], [Name], [Rate]) VALUES (3, N'L-100', N'Energry Lamp 50wt', 500)
GO
SET IDENTITY_INSERT [dbo].[Products] OFF
GO
SET IDENTITY_INSERT [dbo].[InventoryProducts] ON 
GO
INSERT [dbo].[InventoryProducts] ([Id], [InventoryId], [ProductId], [Rate], [Qty], [Discount]) VALUES (4, 5, 1, 56.3, 10, 75)
GO
INSERT [dbo].[InventoryProducts] ([Id], [InventoryId], [ProductId], [Rate], [Qty], [Discount]) VALUES (5, 5, 2, 2960, 1, 360)
GO
INSERT [dbo].[InventoryProducts] ([Id], [InventoryId], [ProductId], [Rate], [Qty], [Discount]) VALUES (6, 5, 3, 500, 5, 500)
GO
INSERT [dbo].[InventoryProducts] ([Id], [InventoryId], [ProductId], [Rate], [Qty], [Discount]) VALUES (7, 8, 1, 56.3, 23, 23)
GO
INSERT [dbo].[InventoryProducts] ([Id], [InventoryId], [ProductId], [Rate], [Qty], [Discount]) VALUES (8, 8, 2, 2960, 1, 0)
GO
INSERT [dbo].[InventoryProducts] ([Id], [InventoryId], [ProductId], [Rate], [Qty], [Discount]) VALUES (9, 10, 2, 2960, 1, 2)
GO
INSERT [dbo].[InventoryProducts] ([Id], [InventoryId], [ProductId], [Rate], [Qty], [Discount]) VALUES (10, 10, 3, 500, 22, 1000)
GO
SET IDENTITY_INSERT [dbo].[InventoryProducts] OFF
GO
