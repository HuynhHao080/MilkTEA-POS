-- ============================================
-- AUDIT LOG SYSTEM - COMPREHENSIVE SETUP
-- Date: April 6, 2026
-- Run in Supabase SQL Editor
-- ============================================

-- ============================================
-- STEP 1: Update audit function (user_id, ip, user_agent)
-- ============================================

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

-- ============================================
-- STEP 2: Audit Triggers for ALL critical tables
-- ============================================

-- Already exist (don't run again):
-- - audit_customers_changes ✅
-- - audit_vouchers_changes ✅
-- - audit_orders_changes ✅
-- - audit_payments_changes ✅

-- NEW TRIGGERS:
CREATE TRIGGER IF NOT EXISTS audit_users_changes
    AFTER INSERT OR UPDATE OR DELETE ON users
    FOR EACH ROW EXECUTE FUNCTION audit_table_changes();

CREATE TRIGGER IF NOT EXISTS audit_roles_changes
    AFTER INSERT OR UPDATE OR DELETE ON roles
    FOR EACH ROW EXECUTE FUNCTION audit_table_changes();

CREATE TRIGGER IF NOT EXISTS audit_categories_changes
    AFTER INSERT OR UPDATE OR DELETE ON categories
    FOR EACH ROW EXECUTE FUNCTION audit_table_changes();

CREATE TRIGGER IF NOT EXISTS audit_products_changes
    AFTER INSERT OR UPDATE OR DELETE ON products
    FOR EACH ROW EXECUTE FUNCTION audit_table_changes();

CREATE TRIGGER IF NOT EXISTS audit_memberships_changes
    AFTER INSERT OR UPDATE OR DELETE ON memberships
    FOR EACH ROW EXECUTE FUNCTION audit_table_changes();

CREATE TRIGGER IF NOT EXISTS audit_toppings_changes
    AFTER INSERT OR UPDATE OR DELETE ON toppings
    FOR EACH ROW EXECUTE FUNCTION audit_table_changes();

CREATE TRIGGER IF NOT EXISTS audit_tables_changes
    AFTER INSERT OR UPDATE OR DELETE ON tables
    FOR EACH ROW EXECUTE FUNCTION audit_table_changes();

-- ============================================
-- STEP 3: Verification Query
-- ============================================

SELECT 
    trigger_name,
    event_object_table AS table_name,
    event_manipulation AS operation,
    action_timing
FROM information_schema.triggers
WHERE trigger_name LIKE 'audit_%'
ORDER BY event_object_table, event_manipulation;

-- ============================================
-- EXPECTED OUTPUT (12 tables audited):
-- ============================================
-- categories | INSERT/UPDATE/DELETE
-- customers  | INSERT/UPDATE/DELETE
-- memberships| INSERT/UPDATE/DELETE
-- orders     | INSERT/UPDATE/DELETE
-- payments   | INSERT/UPDATE/DELETE
-- products   | INSERT/UPDATE/DELETE
-- roles      | INSERT/UPDATE/DELETE
-- tables     | INSERT/UPDATE/DELETE
-- toppings   | INSERT/UPDATE/DELETE
-- users      | INSERT/UPDATE/DELETE
-- vouchers   | INSERT/UPDATE/DELETE
