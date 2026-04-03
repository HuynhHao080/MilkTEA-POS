-- Fix: Make password column nullable since we only use password_hash now
-- Run this in Supabase SQL Editor

ALTER TABLE users ALTER COLUMN password DROP NOT NULL;

-- Verify the change
SELECT column_name, is_nullable 
FROM information_schema.columns 
WHERE table_name = 'users' AND column_name = 'password';
