import MathTool

print(MathTool.tick_to_price(85176))
print(MathTool.price_to_tick(4999.904785770063))

print(MathTool.tick_to_sqrtPriceX96(85176))
print(MathTool.sqrtPriceX96_to_price(5602223755574694718532371873792))

print(MathTool.mint(1, 5000, 4545, 5000, 5500))
print(MathTool.mint(0.75, (5000 * 75) / 100, 4000, 5000, 6250))