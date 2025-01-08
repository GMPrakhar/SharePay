export class TransactionModel {
    from_user!: string;
    total_amount!: number;
    description!: string;
    category!: TransactionCategory;
    transaction_info!: number;
    created_at!: Date;
}

export enum TransactionCategory {
    // Define your categories here
    
    General = 0,
    Food = 1,
    Travel = 2,
    Fashion = 3,
    Grocery = 4,
    Rent = 5,
    Settle = 6
}