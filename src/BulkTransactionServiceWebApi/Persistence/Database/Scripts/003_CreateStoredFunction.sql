CREATE PROCEDURE CheckAvailableFundsForTheTransaction (expense INTEGER, account_iban TEXT) AS $$
declare
	total integer;
BEGIN
    
   SELECT balance_cents into total FROM bank_accounts WHERE iban = account_iban;   
   
    IF total >= expense THEN
        UPDATE bank_accounts SET balance_cents = total - expense WHERE iban = account_iban;
        ELSE
        ASSERT total >= expense, 'Not enough funds on the account.';
    END IF;
END;
$$ LANGUAGE plpgsql;