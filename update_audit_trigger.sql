-- ============================================
-- UPDATE: Fix audit_logs NULL user_id, ip_address, user_agent
-- Date: April 6, 2026
-- Run this in Supabase SQL Editor
-- ============================================

-- Function 11: Audit logging for INSERT/UPDATE/DELETE (IMPROVED)
CREATE OR REPLACE FUNCTION audit_table_changes()
RETURNS TRIGGER AS $$
DECLARE
    v_user_id UUID;
    v_ip_address VARCHAR(45);
    v_user_agent TEXT;
BEGIN
    -- Get session variables (safely handle NULL)
    BEGIN
        v_user_id := current_setting('app.current_user_id', TRUE)::UUID;
    EXCEPTION WHEN OTHERS THEN
        v_user_id := NULL;
    END;

    BEGIN
        v_ip_address := current_setting('app.client_ip', TRUE);
    EXCEPTION WHEN OTHERS THEN
        v_ip_address := NULL;
    END;

    v_user_agent := 'MilkTeaPOS WinForms App';

    IF TG_OP = 'INSERT' THEN
        INSERT INTO audit_logs (user_id, action, table_name, record_id, old_values, new_values, ip_address, user_agent)
        VALUES (v_user_id, TG_OP, TG_TABLE_NAME, NEW.id, NULL, to_jsonb(NEW), v_ip_address, v_user_agent);
        RETURN NEW;
    ELSIF TG_OP = 'UPDATE' THEN
        INSERT INTO audit_logs (user_id, action, table_name, record_id, old_values, new_values, ip_address, user_agent)
        VALUES (v_user_id, TG_OP, TG_TABLE_NAME, NEW.id, to_jsonb(OLD), to_jsonb(NEW), v_ip_address, v_user_agent);
        RETURN NEW;
    ELSIF TG_OP = 'DELETE' THEN
        INSERT INTO audit_logs (user_id, action, table_name, record_id, old_values, new_values, ip_address, user_agent)
        VALUES (v_user_id, TG_OP, TG_TABLE_NAME, OLD.id, to_jsonb(OLD), NULL, v_ip_address, v_user_agent);
        RETURN OLD;
    END IF;
    RETURN NULL;
END;
$$ LANGUAGE plpgsql
SET search_path = public, pg_temp;

-- Update comment
COMMENT ON FUNCTION audit_table_changes IS 'Auto-logs INSERT/UPDATE/DELETE to audit_logs table with user_id, ip_address, user_agent';

-- ============================================
-- VERIFY: Test that session variables work
-- ============================================
DO $$
BEGIN
    -- Test setting session variables
    SET app.current_user_id = '00000000-0000-0000-0000-000000000000';
    SET app.client_ip = '127.0.0.1';

    -- Verify they can be read back
    IF current_setting('app.current_user_id', TRUE)::UUID = '00000000-0000-0000-0000-000000000000' THEN
        RAISE NOTICE '✅ Session variables working correctly';
    ELSE
        RAISE WARNING '⚠️ Session variables may not work properly';
    END IF;
END $$;
