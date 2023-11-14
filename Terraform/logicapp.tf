# resource "azurerm_storage_account" "logicappsa" {
#   count                    = var.deploy_tier ? 1 : 0
#   name                     = "logicazseasa01t"
#   resource_group_name      = azurerm_resource_group.rg_dev[0].name
#   location                 = azurerm_resource_group.rg_dev[0].location
#   account_tier             = var.account_tier
#   account_replication_type = var.account_replication_type
# }

# resource "azurerm_app_service_plan" "aspl" {
#   count               = var.deploy_tier ? 1 : 0
#   name                = "azure-logics-test-service-plan"
#   resource_group_name = azurerm_resource_group.rg_dev[0].name
#   location            = azurerm_resource_group.rg_dev[0].location
#   kind                = "elastic"


#   sku {
#     tier = "WorkflowStandard"
#     size = "WS1"
#   }
# }

# resource "azurerm_logic_app_standard" "testlogicapp" {
#   count                      = var.deploy_tier ? 1 : 0
#   name                       = "test-azure-logic"
#   resource_group_name        = azurerm_resource_group.rg_dev[0].name
#   location                   = azurerm_resource_group.rg_dev[0].location
#   app_service_plan_id        = azurerm_app_service_plan.aspl[0].id
#   storage_account_name       = azurerm_storage_account.logicappsa[0].name
#   storage_account_access_key = azurerm_storage_account.logicappsa[0].primary_access_key

#   app_settings = {
#     "FUNCTIONS_WORKER_RUNTIME"     = "node"
#     "WEBSITE_NODE_DEFAULT_VERSION" = "~18"
#   }

#   identity {
#     type = "SystemAssigned"
#   }
# }