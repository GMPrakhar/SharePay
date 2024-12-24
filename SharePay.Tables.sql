-- Users Table
CREATE TABLE Users (
    user_id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(255) NOT NULL,
    email NVARCHAR(255) UNIQUE NOT NULL
);
GO;

-- Groups Table
CREATE TABLE Groups (
    group_id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(255) NOT NULL,
    setting NVARCHAR(MAX) -- JSON can be stored as NVARCHAR in SQL Server
);
GO;

-- Transactions Table
CREATE TABLE Transactions (
    transaction_id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(255),
    recording_user INT FOREIGN KEY REFERENCES Users(user_id),
    group_id INT FOREIGN KEY REFERENCES Groups(group_id),
    transaction_type NVARCHAR(50) -- Can store values like 'settle' or 'expense'
);
GO;

-- Transaction_Details Table
CREATE TABLE Transaction_Details (
    transaction_detail_id INT IDENTITY(1,1) PRIMARY KEY,
    transaction_id INT FOREIGN KEY REFERENCES Transactions(transaction_id),
    user_id INT FOREIGN KEY REFERENCES Users(user_id),
    owed_amount DECIMAL(10, 2), -- positive or negative
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME DEFAULT GETDATE(),
    created_by_user_id INT FOREIGN KEY REFERENCES Users(user_id),
    updated_by_user_id INT FOREIGN KEY REFERENCES Users(user_id)
);
GO;

-- Group_Users Table
CREATE TABLE Group_Users (
    user_id INT FOREIGN KEY REFERENCES Users(user_id),
    group_id INT FOREIGN KEY REFERENCES Groups(group_id),
    joined_at DATETIME DEFAULT GETDATE(),
    added_by_user_id INT FOREIGN KEY REFERENCES Users(user_id),
    balance DECIMAL(10, 2), -- positive or negative balance
    PRIMARY KEY(user_id, group_id)
);
GO;
