resource "azurerm_service_plan" "fa-asp" {
  count               = var.deploy_tier ? 1 : 0
  name                = local.func_appservice_name
  resource_group_name = azurerm_resource_group.resourcegrp[0].name
  location            = azurerm_resource_group.resourcegrp[0].location
  os_type             = var.os_type
  sku_name            = var.sku_name_func
}

resource "azurerm_service_plan" "la-asp" {
  count               = var.deploy_tier ? 1 : 0
  name                = local.logic_appservice_name
  resource_group_name = azurerm_resource_group.resourcegrp[0].name
  location            = azurerm_resource_group.resourcegrp[0].location
  os_type             = var.os_type
  sku_name            = var.sku_name_logic
}

