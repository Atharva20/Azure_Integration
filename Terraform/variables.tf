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
