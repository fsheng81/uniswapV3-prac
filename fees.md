当LP调用burn()抽取一部分流动性后，可以调用collect()函数将该position中，属于该流动性份额的token返回到recipent地址。考虑到mint()、swap()和position等场景，这种计算方式有一些复杂。

**全局变量**

pool的slot0中，记录有全局的两个globalToken的数量，这个值仅在swap()支付交易费的时候会调用。（如果支付失败呢？）

每一个tick.info中记录有针对该tick的，outsideToken。

position.info中记录 insideLast的值，和 区间中包括的 tokenowned。

**mint场景**

1. 第一次激活该tick：

```solidity
mint() -> _modifyPosition() -> ticks.update()

// ticks.update()
// 如果此时 tick <= currentTick
// outside = global.
// 如果 tick > currentTick 则不变，还是0

// liquidityGross == After.
// liquidityNet = upper ? net + delta : net - delta
```

2. 后续增加/减少tick：

**swap场景**

