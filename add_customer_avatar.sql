-- Add avatar_url column to customers table (similar to categories)

ALTER TABLE customers ADD COLUMN IF NOT EXISTS avatar_url TEXT;

-- Add index for faster lookup
CREATE INDEX IF NOT EXISTS idx_customers_avatar ON customers(avatar_url) WHERE avatar_url IS NOT NULL;

-- Verify
SELECT column_name, data_type, is_nullable 
FROM information_schema.columns 
WHERE table_name = 'customers' AND column_name = 'avatar_url';
