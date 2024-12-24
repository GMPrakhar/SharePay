-- Users Table
CREATE TABLE Users (
    user_id UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    name NVARCHAR(255) NOT NULL,
    email NVARCHAR(255) UNIQUE NOT NULL
);


-- Groups Table
CREATE TABLE Groups (
    group_id UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    owner_id UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Users(user_id),
    description NVARCHAR(MAX),
    name NVARCHAR(255) NOT NULL,
    setting NVARCHAR(MAX) -- JSON can be stored as NVARCHAR in SQL Server
);

-- Transactions Table
CREATE TABLE Transactions (
    transaction_id UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    name NVARCHAR(255),
    recording_user UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Users(user_id),
    group_id UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Groups(group_id),
    transaction_type NVARCHAR(50) -- Can store values like 'settle' or 'expense'
);

-- Transaction_Details Table
CREATE TABLE Transaction_Details (
    transaction_detail_id UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    transaction_id UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Transactions(transaction_id),
    user_id UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Users(user_id),
    owed_amount DECIMAL(10, 2), -- positive or negative
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME DEFAULT GETDATE(),
    created_by_user_id UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Users(user_id),
    updated_by_user_id UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Users(user_id)
);

-- Group_Users Table
CREATE TABLE Group_Users (
    user_id UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Users(user_id),
    group_id UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Groups(group_id),
    joined_at DATETIME DEFAULT GETDATE(),
    added_by_user_id UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Users(user_id),
    balance DECIMAL(10, 2), -- positive or negative balance
    PRIMARY KEY(user_id, group_id)
);
