variable "location" {
  description = "location for all resources."
  type        = string
}

variable "deploy_tier" {
  description = "wether the resource is required in specific environment or not."
  default     = true
}

variable "instance" {
  description = "The count of the resource."
  type        = string
}

variable "env" {
  description = "The environment to deploy resources in."
  type        = string
}

variable "account_tier" {
  description = "The account tier in which storage account will be billed by."
  type        = string
  default     = "Standard"
}

variable "account_replication_type" {
  description = "In how many regions we want the sa to be replicated"
  type        = string
  default     = "LRS"
}

variable "servicebus_ttl" {
  type    = string
  default = "P7D"
}

variable "alert_email" {
  type = string
}