import React from "react";
import styled, { keyframes } from "styled-components";

const tickerAnimation = keyframes`
  0% {
    transform: translateX(-100%);
  }
  100% {
    transform: translateX(100%);
  }
`;

const TickerWrapper = styled.div`
  overflow: hidden;
  white-space: nowrap;
  box-sizing: border-box;
  width: 100%;
`;

const TickerContent = styled.div`
  display: inline-block;
  animation: ${tickerAnimation} 15s linear infinite;
  width: 100%;
`;

const NewsTicker = ({ newsItems }) => {
  return (
    <TickerWrapper>
      <TickerContent>
        {newsItems.map((item, index) => (
          <span
            className="text-white"
            key={index}
            style={{ paddingRight: "30px" }}>
            {item}
          </span>
        ))}
      </TickerContent>
    </TickerWrapper>
  );
};

export default NewsTicker;
