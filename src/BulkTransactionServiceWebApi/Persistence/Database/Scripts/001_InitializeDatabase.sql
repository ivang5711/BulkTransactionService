CREATE TABLE IF NOT EXISTS
    bank_accounts (
        id SERIAL PRIMARY KEY,
        organization_name TEXT NOT NULL,
        balance_cents INT NOT NULL,
        iban TEXT NOT NULL,
        bic TEXT NOT NULL
    );

CREATE TABLE IF NOT EXISTS
    transfers (
        id SERIAL PRIMARY KEY,
        counterparty_name TEXT NOT NULL,
        counterparty_iban TEXT NOT NULL,
        counterparty_bic TEXT NOT NULL,
        amount_cents INT NOT NULL,
        bank_account_id INT NOT NULL,
        description TEXT NOT NULL
    );