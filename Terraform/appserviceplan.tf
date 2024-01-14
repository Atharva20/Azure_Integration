resource "azurerm_service_plan" "fa-asp" {
  count               = var.deploy_tier ? 1 : 0
  name                = local.func_appservice_name
  resource_group_name = azurerm_resource_group.rg_dev[0].name
  location            = azurerm_resource_group.rg_dev[0].location
  os_type             = "Windows"
  sku_name            = "Y1"
}

resource "azurerm_app_service_plan" "la-asp" {
  count               = var.deploy_tier ? 1 : 0
  name                = local.logic_appservice_name
  resource_group_name = azurerm_resource_group.rg_dev[0].name
  location            = azurerm_resource_group.rg_dev[0].location
  kind                = "elastic"

  sku {
    tier = "WorkflowStandard"
    size = "WS1"
  }
}
