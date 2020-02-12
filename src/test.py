from selenium import webdriver
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.common.keys import Keys


driver = webdriver.Chrome("chromedriver.exe")
driver.get("https://localhost/Captcha")
driver.find_element_by_id("submit_form_btn_v2").click()
driver.find_element_by_id("submit_form_btn_v3").click()
driver.implicitly_wait(1000)
driver.close()