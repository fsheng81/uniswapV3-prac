# 实现各种基本的计算函数

import math

min_tick = -887272
max_tick = 887272

q96 = 2**96
eth = 10**18

# price -> tick -> sqrtPriceX96

def price_to_tick(p):
    return math.floor(math.log(p, 1.0001))
def tick_to_price(tick):
    return math.pow(1.0001, tick)

def price_to_sqrtPriceX96(p):
    return int(math.sqrt(p) * q96)
def sqrtPriceX96_to_price(sqrtpx96):
    return math.pow((sqrtpx96 / q96), 2)

def tick_to_sqrtPriceX96(t):
    return int((1.0001 ** (t / 2)) * q96)
def sqrtPriceX96_to_tick(sqrtpx96):
    return int(2 * math.log((sqrtpx96 / q96), 1.0001))

# 计算流动性 / amountX.
def liquidity0_X96(amount, pa, pb):
    if pa > pb:
        pa, pb = pb, pa
    return (amount * (pa * pb) / q96) / (pb - pa)

def liquidity1_X96(amount, pa, pb):
    if pa > pb:
        pa, pb = pb, pa
    return amount * q96 / (pb - pa)

# pa is X96
def calc_amount0(liq, pa, pb):
    if pa > pb:
        pa, pb = pb, pa
    return int(liq * q96 * (pb - pa) / pb / pa)

def calc_amount1(liq, pa, pb):
    if pa > pb:
        pa, pb = pb, pa
    return int(liq * (pb - pa) / q96)

def mint(amount0, amount1, price_low, price_cur, price_upp):
    sqrtp_low = price_to_sqrtPriceX96(price_low)
    sqrtp_cur = price_to_sqrtPriceX96(price_cur)
    sqrtp_upp = price_to_sqrtPriceX96(price_upp)

    amount_0 = amount0 * eth
    amount_1 = amount1 * eth

    liq0 = liquidity0_X96(amount_0, sqrtp_cur, sqrtp_upp)
    liq1 = liquidity1_X96(amount_1, sqrtp_cur, sqrtp_low)
    liq = int(min(liq0, liq1))

    # if price_cur in tick range.
    amount_0_new = calc_amount0(liq, sqrtp_cur, sqrtp_upp) / eth
    amount_1_new = calc_amount1(liq, sqrtp_low, sqrtp_cur) / eth
    return (liq, amount_0_new, amount_1_new)